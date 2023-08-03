using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository;
using FlairTickets.Web.Helpers;
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
        public IActionResult Index()
        {
            return View(_ticketRepository.GetAll());
        }


        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _ticketRepository.GetByIdWithFlightDetailsAsync(id.Value);
            if (ticket == null) return NotFound();

            return View(ticket);
        }


        // GET: Tickets/Create/{flightId : int}
        public async Task<IActionResult> Create(int? flightId)
        {
            if (flightId == null) return NotFound();

            var flight = await _flightRepository.GetByIdAsync(flightId.Value);
            if (flight == null) return NotFound();

            var ticket = new Ticket { Flight = flight };

            return View(ticket);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket, int flightId)
        {
            if (ModelState.IsValid)
            {
                var flight = await _flightRepository.GetByIdAsync(flightId);
                if (flight == null) return NotFound();

                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user == null) return NotFound();

                await _ticketRepository.CreateAsync(ticket, flight, user);
                return RedirectToAction(nameof(Index));
            }

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

    }
}
