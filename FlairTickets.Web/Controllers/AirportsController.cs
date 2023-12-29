using System.Threading.Tasks;
using FlairTickets.Web.Data;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Models;
using FlairTickets.Web.Models.IndexViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AirportsController : Controller
    {
        private readonly IDataUnit _dataUnit;

        public AirportsController(IDataUnit dataUnit)
        {
            _dataUnit = dataUnit;
        }


        // GET: Airports
        public async Task<IActionResult> Index(Airport? searchModel)
        {
            var indexModel = new AirportsIndexViewModel
            {
                SearchModel = searchModel,
                Airports = searchModel == null
                    ? await _dataUnit.Airports.GetAll().ToListAsync()
                    : await _dataUnit.Airports.GetSearchAsync(searchModel)
            };

            return View(indexModel);
        }


        // GET: Airports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Airport airport)
        {
            if (ModelState.IsValid)
            {
                airport.IataCode = airport.IataCode.ToUpper();

                try
                {
                    await _dataUnit.Airports.CreateAsync(airport);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            $"There is already an {nameof(Airport)}" +
                            $" with this {nameof(Airport.IataCode)}.");

                        return View(airport);
                    }
                }
                catch { }
            }

            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Airport)}.");
            return View(airport);
        }


        // GET: Airports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return AirportNotFound();

            var airport = await _dataUnit.Airports.GetByIdAsync(id.Value);
            if (airport == null) return AirportNotFound();

            return View(airport);
        }

        // POST: Airports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Airport airport)
        {
            if (ModelState.IsValid)
            {
                airport.IataCode = airport.IataCode.ToUpper();

                try
                {
                    await _dataUnit.Airports.UpdateAsync(airport);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _dataUnit.Airports.ExistsAsync(airport.Id))
                    {
                        return AirportNotFound();
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            $"There is already an {nameof(Airport)}" +
                            $" with this {nameof(Airport.IataCode)}.");

                        return View(airport);
                    }
                }
                catch { }
            }

            ModelState.AddModelError(string.Empty, $"Could not update {nameof(Airport)}.");
            return View(airport);
        }


        // GET: Airports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return AirportNotFound();

            var airport = await _dataUnit.Airports.GetByIdAsync(id.Value);
            if (airport == null) return AirportNotFound();

            return View(airport);
        }

        // POST: Airports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _dataUnit.Airports.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException innerEx)
                {
                    if (innerEx.Message.Contains("FK_Flights_Airports_"))
                    {
                        TempData["LayoutMessage"] =
                            "Could not delete Airport." +
                            " There's at least one Flight depending on it.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch { }

            TempData["LayoutMessage"] = "Could not delete Airport.";
            return RedirectToAction(nameof(Index));
        }


        public IActionResult AirportNotFound()
        {
            var model = new NotFoundViewModel
            {
                Title = $"{nameof(Airport)} not found",
                EntityName = nameof(Airport),
            };
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View(nameof(HomeController.NotFound), model);
        }
    }
}
