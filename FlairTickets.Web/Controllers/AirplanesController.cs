using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Controllers
{
    [Authorize]
    public class AirplanesController : Controller
    {
        private readonly IAirplaneRepository _airplaneRepository;

        public AirplanesController(IAirplaneRepository airplaneRepository)
        {
            _airplaneRepository = airplaneRepository;
        }


        // GET: Airplanes
        public IActionResult Index()
        {
            return View(_airplaneRepository.GetAll());
        }


        // GET: Airplanes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null) return NotFound();

            return View(airplane);
        }


        // GET: Airplanes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airplanes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Model,Name,Seats")] Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                await _airplaneRepository.CreateAsync(airplane);
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Airplane)}.");
            return View(airplane);
        }


        // GET: Airplanes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null) return NotFound();

            return View(airplane);
        }

        // POST: Airplanes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Name,Seats")] Airplane airplane)
        {
            if (id != airplane.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _airplaneRepository.UpdateAsync(airplane);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _airplaneRepository.ExistsAsync(airplane.Id))
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

            ModelState.AddModelError(string.Empty, $"Could not update {nameof(Airplane)}.");
            return View(airplane);
        }


        // GET: Airplanes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null) return NotFound();

            return View(airplane);
        }

        // POST: Airplanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _airplaneRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
