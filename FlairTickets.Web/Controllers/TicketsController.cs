using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;
using FlairTickets.Web.Helpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IFlightRepository _flightRepository;
        private readonly ITicketRepository _ticketRepository;

        public TicketsController(
            IUserHelper userHelper,
            IFlightRepository flightRepository,
            ITicketRepository ticketRepository)
        {
            _userHelper = userHelper;
            _flightRepository = flightRepository;
            _ticketRepository = ticketRepository;
        }


        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
        {
                // Success.
            return View(_ticketRepository.GetAll());
        }

            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null) return NotFound();

            // Success.
            return View(_ticketRepository.GetAllOfUser(user));
        }


        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _ticketRepository.GetByIdWithFlightDetailsAsync(id.Value);
            if (ticket == null) return NotFound();

            return View(ticket);
        }


        // GET: Tickets/Create/{flightId}
        public async Task<IActionResult> Create(int? flightId)
        {
            if (flightId == null) return NotFound();

            var flight = await _flightRepository.GetByIdAsync(flightId.Value);
            if (flight == null) return NotFound();

            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null) return NotFound();

            var ticket = new Ticket { Flight = flight, User = user };

            return View(ticket);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket, int flightId)
        {
            // Get Flight from which Ticket was created.
            var flight = await _flightRepository.GetByIdAsync(flightId);
            if (flight == null) return NotFound();

            if (ModelState.IsValid)
            {
                // Check Seat exists in Flight.
                if (!await _flightRepository.IsSeatInBoundsAsync(flight.Id, ticket.Seat))
                {
                    ModelState.AddModelError(
                        string.Empty,
                        $"This {nameof(Ticket.Seat)} doesn't exist in this {nameof(Flight)}.");

                    ticket.Flight = flight;
                    return View(ticket);
                }

                // Check Seat is available.
                if (await _ticketRepository.IsSeatTakenAsync(flight.Id, ticket.Seat, 0))
                {
                    ModelState.AddModelError(
                        string.Empty, $"This {nameof(Ticket.Seat)} is not available.");

                    ticket.Flight = flight;
                    return View(ticket);
                }

                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user == null) return NotFound();

                await _ticketRepository.CreateAsync(ticket, flight, user);

                // Success.
                return RedirectToAction(nameof(Details), new { id = ticket.Id });
            }

            ticket.Flight = flight;
            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Ticket)}.");
            return View(ticket);
        }


        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _ticketRepository.GetByIdWithFlightDetailsAsync(id.Value);
            if (ticket == null) return NotFound();

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _ticketRepository.UpdateAsync(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _ticketRepository.ExistsAsync(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Ticket)}.");
            return View(ticket);
        }


        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _ticketRepository.GetByIdWithFlightDetailsAsync(id.Value);
            if (ticket == null) return NotFound();

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ticketRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> CheckSeat(int flightId, int seat, int ticketId)
        {
            try
            {
                if (flightId < 1)
                    return Json("Invalid flight.");

                if (seat < 1)
                    return Json("Invalid seat.");

                if (ticketId < 0)
                    return Json("Invalid ticket.");

                if (!await _flightRepository.IsSeatInBoundsAsync(flightId, seat))
                    return Json("This seat doesn't exist in this flight.");

                if (await _ticketRepository.IsSeatTakenAsync(flightId, seat, ticketId))
                    return Json(false);

                return Json(true);
            }
            catch
            {
                return Json("error");
            }
        }

    }
}
