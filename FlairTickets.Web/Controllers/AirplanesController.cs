﻿using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;
using FlairTickets.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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
            if (id == null) return AirplaneNotFound();

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null) return AirplaneNotFound();

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
        public async Task<IActionResult> Create(Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _airplaneRepository.CreateAsync(airplane);
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
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, $"Could not create {nameof(Airplane)}.");
            return View(airplane);
        }


        // GET: Airplanes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return AirplaneNotFound();

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
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
                    await _airplaneRepository.UpdateAsync(airplane);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _airplaneRepository.ExistsAsync(airplane.Id))
                    {
                        return AirplaneNotFound();
                    }
                    else
                    {
                        throw;
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
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, $"Could not update {nameof(Airplane)}.");
            return View(airplane);
        }


        // GET: Airplanes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return AirplaneNotFound();

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null) return AirplaneNotFound();

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
