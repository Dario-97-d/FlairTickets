using System;
﻿using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers.ControllerHelpers.Interfaces;
using FlairTickets.Web.Models;
using FlairTickets.Web.Models.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Controllers
{
    [Authorize(Roles = "Customer,Admin")]
    public class TicketsController : Controller
    {
        private readonly IControllerHelpers _controllerHelper;

        public TicketsController(IControllerHelpers controllerHelper)
        {
            _controllerHelper = controllerHelper;
        }


        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var tickets = User.IsInRole("Admin")
                ? await _controllerHelper.Tickets.GetTicketsForIndexAsync()
                : await _controllerHelper.Tickets.GetTicketsForIndexAsync(User.Identity.Name);

            return View(tickets);
        }


        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return TicketNotFound();

            var model = User.IsInRole("Admin")
                ? await _controllerHelper.Tickets.GetViewModelForDisplayAsync(id.Value)
                : await _controllerHelper.Tickets
                    .GetViewModelForDisplayAsync(id.Value, User.Identity.Name);

            if (model == null) return TicketNotFound();

            return View(model);
        }


        // GET: Tickets/Create?flightId={flightId}
        public async Task<IActionResult> Create(int? flightId)
        {
            if (flightId == null) return FlightNotFound();

            var model = await _controllerHelper.Tickets
                .GetViewModelForInputCreateAsync(flightId.Value, User.Identity.Name);

            if (model == null) return TicketNotFound();

            return View(model);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InputTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check the model email is the real email of the logged user.
                if (model.UserEmail != User.Identity.Name)
                {
                    TempData["LayoutMessage"] = "You must login to your account to buy a ticket.";
                    return RedirectToAction(nameof(AccountController.Logout), "Account");
                }

                // Check ticket is valid.
                var checkTicketMessage = await _controllerHelper.Tickets.CheckTicketAsync(model, "create");

                if (checkTicketMessage != string.Empty)
                {
                    ModelState.AddModelError(string.Empty, checkTicketMessage);
                    return View(model);
                }

                try
                {
                    var ticketId = await _controllerHelper.Tickets.CreateTicketAsync(model);

                // Success.
                    return RedirectToAction(nameof(Details), new { id = ticketId });
            }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                    {
                        ModelState.AddModelError(string.Empty, "This seat is unavailable.");
                        return View(model);
                    }
                }
            }

            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Ticket)}.");
            return View(model);
        }


        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return TicketNotFound();

            var model = await _controllerHelper.Tickets
                .GetViewModelForInputEditAsync(id.Value, User.Identity.Name);

            if (model == null) return TicketNotFound();

            return View(model);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InputTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check the model email is the real email of the logged user.
                if (model.UserEmail != User.Identity.Name)
                {
                    TempData["LayoutMessage"] = "You must login to your account to change your seat.";
                    return RedirectToAction(nameof(AccountController.Logout), "Account");
                }

                // Check ticket is valid.
                var checkTicketMessage = await _controllerHelper.Tickets.CheckTicketAsync(model, "update");

                if (checkTicketMessage != string.Empty)
                {
                    ModelState.AddModelError(string.Empty, checkTicketMessage);
                    return View(model);
                }

                try
                {
                    var ticketId = await _controllerHelper.Tickets.UpdateTicketAsync(model);
                    return RedirectToAction(nameof(Details), new { id = ticketId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _controllerHelper.Tickets.TicketExistsAsync(model.Id))
                        return TicketNotFound();
                    }
                catch (Exception ex)
                    {
                    if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                    {
                        ModelState.AddModelError(string.Empty, "This seat is unavailable.");
                        return View(model);
                    }
                }
            }

            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Ticket)}.");
            return View(model);
        }


        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return TicketNotFound();

            var model = await _controllerHelper.Tickets.GetViewModelForDisplayAsync(id.Value);
            if (model == null) return TicketNotFound();

            return View(model);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _controllerHelper.Tickets.DeleteTicketAsync(id);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult FlightNotFound()
        {
            return RedirectToAction(nameof(FlightsController.FlightNotFound), "Flights");
        }

        public IActionResult TicketNotFound()
        {
            var model = new NotFoundViewModel
            {
                Title = $"{nameof(Ticket)} not found",
                EntityName = nameof(Ticket),
            };
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View(nameof(HomeController.NotFound), model);
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

                if (!await _controllerHelper.Tickets.IsSeatInBoundsAsync(flightId, seat))
                    return Json("This seat doesn't exist in this flight.");

                if (await _controllerHelper.Tickets.IsSeatTakenAsync(flightId, seat, ticketId))
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
