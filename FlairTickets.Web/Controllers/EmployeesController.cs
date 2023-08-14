using System;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers.Interfaces;
using FlairTickets.Web.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlairTickets.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly IMailHelper _mailHelper;
        private readonly IUserHelper _userHelper;

        public EmployeesController(IMailHelper mailHelper, IUserHelper userHelper)
        {
            _mailHelper = mailHelper;
            _userHelper = userHelper;
        }


        // GET: EmployeesController
        public async Task<ActionResult> Index()
        {
            return View(await _userHelper.GetAllInRoleAsync("Employee"));
        }


        // GET: EmployeesController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }


        // GET: EmployeesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check e-mail address is registered.
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already registered.");
                    return View(model);
                }

                user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    ChosenName = model.ChosenName,
                    FullName = model.FullName,
                    Document = model.Document,
                    Address = model.Address,
                };

                var password = Guid.NewGuid().ToString() + "Upper";

                var addUser = await _userHelper.AddUserAsync(user, password);
                if (addUser.Succeeded)
                {
                    await _userHelper.AddUserToRoleAsync(user, "Employee");
                    
                    string token = await _userHelper.GeneratePasswordResetTokenAsync(user);

                    string tokenUrl = Url.Action(
                        action: nameof(AccountController.ResetPassword),
                        controller: "Account",
                        values: new { token },
                        protocol: HttpContext.Request.Scheme);

                    var sendPasswordResetEmail = _mailHelper.SendPasswordResetEmail(user, tokenUrl);
                    if (sendPasswordResetEmail)
                    {
                        // Success.

                        ViewBag.Message =
                            $"An email has been sent to <i>{model.Email}</i> " +
                            $"with a link to reset password.";

                        return View();
                    }

                    // If it gets here, rollback user creation.
                    await _userHelper.RollbackRegisteredUserAsync(user);

                    // Could not send password reset email.
                    ModelState.AddModelError(string.Empty, "Could not send password reset email.");
                    return View(model);
                }

                // Could not add user.
                ModelState.AddModelError(string.Empty, "Could not add user.");
                return View(model);
            }

            // ModelState is not valid.
            ModelState.AddModelError(string.Empty, "Could not add user. Check input and try again.");
            return View(model);
        }


        // GET: EmployeesController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var model = new EditEmployeeViewModel
            {
                Id = user.Id,
                Email = user.Email,
                ChosenName = user.ChosenName,
            };

            return View(model);
        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check email is already registered.
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "This email address is already registered." +
                        " Choose another one or change the existing account's.");

                    return View(model);
                }

                user = await _userHelper.GetUserByIdAsync(model.Id);
                if (user == null) return NotFound();

                // Update Employee.
                user.UserName = model.Email;
                user.Email = model.Email;
                user.EmailConfirmed = false;

                var updateEmployee = await _userHelper.UpdateUserAsync(user);
                if (updateEmployee.Succeeded)
                {
                    string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                    string tokenUrl = Url.Action(
                        action: nameof(AccountController.ConfirmEmail),
                        controller: "Account",
                        values: new
                        {
                            userid = user.Id,
                            token
                        },
                        protocol: HttpContext.Request.Scheme);

                    if (!string.IsNullOrEmpty(tokenUrl))
                    {
                        var sendConfirmationEmail = _mailHelper.SendConfirmationEmail(user, tokenUrl);
                        if (sendConfirmationEmail)
                        {
                            // Success.

                            ViewBag.Message =
                                $"{model.ChosenName}'s email address has been updated, " +
                                $"and a confirmation email with a link has been sent." +
                                $" In order to access the account, " +
                                $"{model.ChosenName} needs to activate it through that link.";

                            return View(model);
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Could not update Employee.");
                return View(model);
            }

            ModelState.AddModelError(
                string.Empty,
                "Input was not accepted. Correct it and try again.");

            return View(model);
        }


        // GET: EmployeesController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: EmployeesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, bool confirmed = true)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            await _userHelper.RemoveFromRoleAsync(user, "Employee");
            
            try
            {
                TempData["Message"] = $"{user.ChosenName} has been removed from Employee status.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
