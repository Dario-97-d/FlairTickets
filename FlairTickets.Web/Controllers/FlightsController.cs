using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository;
using FlairTickets.Web.Helpers;
using FlairTickets.Web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Controllers
{
    [Authorize]
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
            return View(_flightRepository.GetAll());
        }


        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null) return NotFound();

            return View(flight);
        }


        // GET: Flights/Create
        public IActionResult Create()
        {
            var model = new FlightViewModel
            {
                Airports = _airportRepository.GetComboAirports(),
                Airplanes = _airplaneRepository.GetComboAirplanes()
            };
            return View(model);
        }

        // POST: Flights/Create
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null) return NotFound();

            var model = _converterHelper.FlightToViewModel(flight);

            model.Airplanes = _airplaneRepository.GetComboAirplanes();
            model.Airports = _airportRepository.GetComboAirports();

            return View(model);
        }

        // POST: Flights/Edit/5
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
                        return NotFound();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null) return NotFound();

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _flightRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
