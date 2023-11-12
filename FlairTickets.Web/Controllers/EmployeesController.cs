using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers.Interfaces;
using FlairTickets.Web.Models;
using FlairTickets.Web.Models.Account;
using FlairTickets.Web.Models.IndexViewModels;
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
        public async Task<IActionResult> Index(User? searchModel)
        {
            var indexModel = new EmployeesIndexViewModel
            {
                SearchModel = searchModel,
                Employees = searchModel == null
                    ? await _userHelper.GetAllInRoleAsync("Employee")
                    : await _userHelper.GetSearchInRoleAsync(searchModel, "Employee")
            };

            return View(indexModel);
        }


        // GET: EmployeesController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return EmployeeNotFound();

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null) return EmployeeNotFound();

            if (string.IsNullOrEmpty(user.PhoneNumber))
            {
                user.PhoneNumber = "n/a";
            }

            return View(user);
        }


        // GET: EmployeesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeViewModel model)
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

                var addUser = await _userHelper.AddUserAsync(user);
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
                    await _userHelper.DeleteUserAsync(user);

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
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return EmployeeNotFound();

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null) return EmployeeNotFound();

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
        public async Task<IActionResult> Edit(EditEmployeeViewModel model)
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
                if (user == null) return EmployeeNotFound();

                // Update Employee.
                user.UserName = model.Email;
                user.Email = model.Email;
                user.EmailConfirmed = false;

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
                        var updateEmployee = await _userHelper.UpdateUserAsync(user);
                        if (updateEmployee.Succeeded)
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
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return EmployeeNotFound();

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null) return EmployeeNotFound();

            return View(user);
        }

        // POST: EmployeesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, bool confirmed = true)
        {
            if (string.IsNullOrEmpty(id)) return EmployeeNotFound();

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null) return EmployeeNotFound();

            await _userHelper.DeleteUserAsync(user);

            TempData["Message"] = $"{user.ChosenName}'s account has been deleted.";
            return RedirectToAction(nameof(Index));
        }


        public IActionResult EmployeeNotFound()
        {
            var model = new NotFoundViewModel
            {
                Title = "Employee not found",
                EntityName = "Employee",
            };
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View(nameof(HomeController.NotFound), model);
        }
    }
}
