using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Controllers
{
    [Authorize]
    public class AirportsController : Controller
    {
        private readonly IAirportRepository _airportRepository;

        public AirportsController(IAirportRepository airportRepository)
        {
            _airportRepository = airportRepository;
        }


        // GET: Airports
        public IActionResult Index()
        {
            return View(_airportRepository.GetAll());
        }


        // GET: Airports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var airport = await _airportRepository.GetByIdAsync(id.Value);
            if (airport == null) return NotFound();

            return View(airport);
        }


        // GET: Airports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IataCode,Name,City,Country")] Airport airport)
        {
            if (ModelState.IsValid)
            {
                await _airportRepository.CreateAsync(airport);
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Airport)}.");
            return View(airport);
        }


        // GET: Airports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var airport = await _airportRepository.GetByIdAsync(id.Value);
            if (airport == null) return NotFound();

            return View(airport);
        }

        // POST: Airports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IataCode,Name,City,Country")] Airport airport)
        {
            if (id != airport.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _airportRepository.UpdateAsync(airport);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _airportRepository.ExistsAsync(airport.Id))
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

            ModelState.AddModelError(string.Empty, $"Could not update {nameof(Airport)}.");
            return View(airport);
        }


        // GET: Airports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var airport = await _airportRepository.GetByIdAsync(id.Value);
            if (airport == null) return NotFound();

            return View(airport);
        }

        // POST: Airports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _airportRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
