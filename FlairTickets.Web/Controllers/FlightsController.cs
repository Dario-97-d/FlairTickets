using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;
using FlairTickets.Web.Helpers.Interfaces;
using FlairTickets.Web.Models;
using FlairTickets.Web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IConverterHelper _converterHelper;

        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IAirportRepository _airportRepository;
        private readonly IFlightRepository _flightRepository;

        public FlightsController(
            IConverterHelper converterHelper,
            IAirplaneRepository airplaneRepository,
            IAirportRepository airportRepository,
            IFlightRepository flightRepository)
        {
            _converterHelper = converterHelper;

            _airplaneRepository = airplaneRepository;
            _airportRepository = airportRepository;
            _flightRepository = flightRepository;
        }


        // GET: Flights
        public IActionResult Index()
        {
            // Assign role for partial view of index table
            if (!User.Identity.IsAuthenticated)
                ViewBag.Role = "Offline";
            else if (User.IsInRole("Employee"))
                ViewBag.Role = "Employee";
            else
                ViewBag.Role = "Customer";

            return View(_flightRepository.GetAll());
        }


        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return FlightNotFound();

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null) return FlightNotFound();

            if (User.Identity.IsAuthenticated)
                return View(flight);
            else
                return View("NotAuthDetails", flight);
        }


        // GET: Flights/Create
        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            var model = new FlightViewModel
            {
                ComboAirports = _airportRepository.GetComboAirports(),
                ComboAirplanes = _airplaneRepository.GetComboAirplanes()
            };
            return View(model);
        }

        // POST: Flights/Create
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                var flight = await _converterHelper.ViewModelToFlightAsync(model);
                await _flightRepository.CreateAsync(flight);
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Flight)}.");
            return View(model);
        }


        // GET: Flights/Edit/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return FlightNotFound();

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null) return FlightNotFound();

            var model = _converterHelper.FlightToViewModel(flight);

            model.ComboAirplanes = _airplaneRepository.GetComboAirplanes();
            model.ComboAirports = _airportRepository.GetComboAirports();

            return View(model);
        }

        // POST: Flights/Edit/5
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                var flight = await _converterHelper.ViewModelToFlightAsync(model);

                try
                {
                    await _flightRepository.UpdateAsync(flight);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _flightRepository.ExistsAsync(model.Id))
                    {
                        return FlightNotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ModelState.AddModelError(string.Empty, $"Could not update {nameof(Flight)}.");
            return View(model);
        }


        // GET: Flights/Delete/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return FlightNotFound();

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null) return FlightNotFound();

            return View(flight);
        }

        // POST: Flights/Delete/5
        [Authorize(Roles = "Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _flightRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult FlightNotFound()
        {
            var model = new NotFoundViewModel
            {
                Title = $"{nameof(Flight)} not found",
                EntityName = nameof(Flight),
            };
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View(nameof(HomeController.NotFound), model);
        }
    }
}
