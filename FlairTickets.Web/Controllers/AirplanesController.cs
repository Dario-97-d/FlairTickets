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
    public class AirplanesController : Controller
    {
        private readonly IDataUnit _dataUnit;

        public AirplanesController(IDataUnit dataUnit)
        {
            _dataUnit = dataUnit;
        }


        // GET: Airplanes
        public async Task<IActionResult> Index(Airplane? searchModel)
        {
            var indexModel = new AirplanesIndexViewModel
            {
                SearchModel = searchModel,
                Airplanes = searchModel == null
                    ? await _dataUnit.Airplanes.GetAll().ToListAsync()
                    : await _dataUnit.Airplanes.GetSearchAsync(searchModel)
            };

            return View(indexModel);
        }


        // GET: Airplanes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airplanes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _dataUnit.Airplanes.CreateAsync(airplane);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            $"There is already an {nameof(Airplane)}" +
                            $" with this {nameof(Airplane.Name)}.");

                        return View(airplane);
                    }
                }
                catch { }
            }

            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Airplane)}.");
            return View(airplane);
        }


        // GET: Airplanes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return AirplaneNotFound();

            var airplane = await _dataUnit.Airplanes.GetByIdAsync(id.Value);
            if (airplane == null) return AirplaneNotFound();

            return View(airplane);
        }

        // POST: Airplanes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _dataUnit.Airplanes.UpdateAsync(airplane);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _dataUnit.Airplanes.ExistsAsync(airplane.Id))
                    {
                        return AirplaneNotFound();
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            $"There is already an {nameof(Airplane)}" +
                            $" with this {nameof(Airplane.Name)}.");

                        return View(airplane);
                    }
                }
                catch { }
            }

            ModelState.AddModelError(string.Empty, $"Could not update {nameof(Airplane)}.");
            return View(airplane);
        }


        // GET: Airplanes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return AirplaneNotFound();

            var airplane = await _dataUnit.Airplanes.GetByIdAsync(id.Value);
            if (airplane == null) return AirplaneNotFound();

            return View(airplane);
        }

        // POST: Airplanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _dataUnit.Airplanes.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException innerEx)
                {
                    if (innerEx.Message.Contains("FK_Flights_Airplanes_AirplaneId"))
                    {
                        TempData["LayoutMessage"] =
                            "Could not delete Airplane." +
                            " There's at least one Flight depending on it.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch { }

            TempData["LayoutMessage"] = "Could not delete Airplane.";
            return RedirectToAction(nameof(Index));
        }


        public IActionResult AirplaneNotFound()
        {
            var model = new NotFoundViewModel
            {
                Title = "Airplane not found",
                EntityName = nameof(Airplane),
            };
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View(nameof(HomeController.NotFound), model);
        }
    }
}
