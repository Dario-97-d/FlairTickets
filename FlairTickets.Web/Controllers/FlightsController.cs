using System.Collections.Generic;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers.ControllerHelpers.Interfaces;
using FlairTickets.Web.Models;
using FlairTickets.Web.Models.Flight;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IControllerHelpers _controllerHelper;

        public FlightsController(IControllerHelpers controllerHelper)
        {
            _controllerHelper = controllerHelper;
        }


        // GET: Flights
        public async Task<IActionResult> Index(SearchFlightViewModel? searchModel)
        {
            IndexFlightViewModel model = null;

            try
            {
                model = await _controllerHelper.Flights.GetViewModelForIndexAsync(searchModel);
            }
            catch (System.FormatException ex)
            {
                TempData["LayoutMessage"] = ex.Message switch
                {
                    "Date format error" => $"Date format error. Re-enter date and try again.",
                    "Time format error" => $"Hour format error. Re-enter hour and try again.",
                    _ => null
                };
            }
            catch
            {
                TempData["LayoutMessage"] = "Something went wrong.";
            }

            model ??= new IndexFlightViewModel
            {
                Flights = new List<IndexRowFlightViewModel>(),
                SearchFlight = searchModel,
            };

            // Assign role for partial view of index table
            if (!User.Identity.IsAuthenticated)
                ViewBag.Role = "Offline";
            else if (User.IsInRole("Employee"))
                ViewBag.Role = "Employee";
            else
                ViewBag.Role = "Customer";

            ViewBag.ComboAirports = await _controllerHelper.Flights.GetComboAirportsIataCodeAsync();

            return View(nameof(Index), model);
        }


        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return FlightNotFound();

            var model = await _controllerHelper.Flights.GetViewModelForDisplayAsync(id.Value);
            if (model == null) return FlightNotFound();

            return View(model);
        }


        // GET: Flights/Create
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create()
        {
            (ViewBag.ComboAirports, ViewBag.ComboAirplanes) =
                await _controllerHelper.Flights.GetComboAirportsAndAirplanesAsync();

            return View();
        }

        // POST: Flights/Create
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InputFlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _controllerHelper.Flights.CreateFlightAsync(model);

                    if (model.OriginAirportIataCode == model.DestinationAirportIataCode)
                    {
                        TempData["LayoutMessage"] =
                            "The flight just created has the same Airport" +
                            " for both Origin and Destination.";
                    }
                return RedirectToAction(nameof(Index));
            }
                catch { }
            }

            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Flight)}.");

            (ViewBag.ComboAirports, ViewBag.ComboAirplanes) =
                            await _controllerHelper.Flights.GetComboAirportsAndAirplanesAsync();

            return View(model);
        }


        // GET: Flights/Edit/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return FlightNotFound();

            var model = await _controllerHelper.Flights.GetViewModelForInputAsync(id.Value);
            if (model == null) return FlightNotFound();

            (ViewBag.ComboAirports, ViewBag.ComboAirplanes) =
                await _controllerHelper.Flights.GetComboAirportsAndAirplanesAsync();

            return View(model);
        }

        // POST: Flights/Edit/5
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InputFlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _controllerHelper.Flights.UpdateFlightAsync(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _controllerHelper.Flights.FlightExistsAsync(model.Id))
                        return FlightNotFound();
                    }
                catch { }
                    }

            ModelState.AddModelError(string.Empty, $"Could not update {nameof(Flight)}.");

            (ViewBag.ComboAirports, ViewBag.ComboAirplanes) =
                await _controllerHelper.Flights.GetComboAirportsAndAirplanesAsync();

            return View(model);
        }


        // GET: Flights/Delete/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return FlightNotFound();

            var model = await _controllerHelper.Flights.GetViewModelForDisplayAsync(id.Value);
            if (model == null) return FlightNotFound();

            return View(model);
        }

        // POST: Flights/Delete/5
        [Authorize(Roles = "Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _controllerHelper.Flights.DeleteFlightAsync(id);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Search(SearchFlightViewModel model)
        {
            return await Index(model);
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
