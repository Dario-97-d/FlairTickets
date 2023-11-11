using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers.Interfaces;
using FlairTickets.Web.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlairTickets.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMailHelper _mailHelper;
        private readonly IUserHelper _userHelper;

        public AccountController(IMailHelper mailHelper, IUserHelper userHelper)
        {
            _mailHelper = mailHelper;
            _userHelper = userHelper;
        }


        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var changePassword = await _userHelper.ChangePasswordAsync(
                        user, model.OldPassword, model.NewPassword);

                    if (changePassword.Succeeded)
                    {
                        ViewBag.UserMessage = "Password updated!";
                        return View();
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Could not update password.");
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userid);
            if (user == null) return NotFound();

            var confirmEmail = await _userHelper.ConfirmEmailAsync(user, token);
            if (confirmEmail.Succeeded)
            {
                await _userHelper.AddUserToRoleAsync(user, "Customer");

                return View("EmailConfirmed");
            }

            ViewBag.Message = "Could not confirm email.";
            return View();
        }

        public IActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated) return RedirectToHomePage();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (User.Identity.IsAuthenticated) return RedirectToHomePage();

            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    string token = await _userHelper.GeneratePasswordResetTokenAsync(user);

                    string tokenUrl = Url.Action(
                        action: nameof(ResetPassword),
                        controller: "Account",
                        values: new { token },
                        protocol: HttpContext.Request.Scheme);

                    if (!string.IsNullOrEmpty(tokenUrl))
                    {
                        var sendPasswordResetEmail = _mailHelper.SendPasswordResetEmail(user, tokenUrl);
                        if (sendPasswordResetEmail)
                        {
                            // Success.

                            TempData["Message"] =
                                $"An email has been sent to <i>{model.Email}</i> " +
                                $"with a link to reset password.";

                            return RedirectToHomePage();
                        }
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Could not send password reset email.");
            return View(nameof(ForgotPassword), model);
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                var model = new UpdateUserViewModel
                {
                    ChosenName = user.ChosenName,
                    FullName = user.FullName,
                    Document = user.Document,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber
                };

                return View(model);
            }

            return View();
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToHomePage();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated) return RedirectToHomePage();

            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    var login = await _userHelper.LoginAsync(user, model.Password, model.RememberMe);
                    if (login.Succeeded)
                    {
                        if (Request.Query.ContainsKey("ReturnUrl"))
                        {
                            return Redirect(Request.Query["ReturnUrl"]);
                        }
                        else return RedirectToHomePage();
                    }

                    // Check why login failed and display message accordingly.
                    if (login.IsNotAllowed)
                    {
                        // -- For employees, email confirmation is made trough password reset.
                        // Hence, it's better to check for password length before email confirmation,
                        // for employees to get better feedback on why their login failed. --

                        // Has not password?
                        if (user.PasswordHash == null)
                        {
                            ModelState.AddModelError(string.Empty, "You need to set a password to login.");
                            return View();
                        }

                        // Is email confirmation not done?
                        if (!user.EmailConfirmed)
                        {
                            ModelState.AddModelError(string.Empty, "You need to confirm email.");
                            return View();
                }
            }
                }
            }

            ModelState.AddModelError(string.Empty, "Could not login.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
                await _userHelper.LogoutAsync();

            return RedirectToHomePage();
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToHomePage();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (User.Identity.IsAuthenticated) return RedirectToHomePage();

            if (ModelState.IsValid)
            {
                // Check User already exists
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    user = new User
                    {
                        Email = model.Email,
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        ChosenName = model.ChosenName,
                        FullName = model.FullName,
                        Document = model.Document,
                        Address = model.Address
                    };

                    var registerUser = await _userHelper.AddUserAsync(user, model.Password);
                    if (registerUser.Succeeded)
                    {
                        string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                        string tokenUrl = Url.Action(
                            action: nameof(ConfirmEmail),
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

                                ViewBag.Message = "Hi there!" +
                                    " In order to access the account, email confirmation is required." +
                                    " A link has been sent to the registered email address.";

                                return View(nameof(ConfirmEmail));
                            }
                        }

                        // If it gets here, rollback user creation.
                        await _userHelper.DeleteUserAsync(user);
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Could not sign up.");
            return View();
        }

        public IActionResult ResetPassword(string token)
        {
            if (User.Identity.IsAuthenticated) return RedirectToHomePage();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (User.Identity.IsAuthenticated) return RedirectToHomePage();

            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    var resetPassword = await _userHelper.ResetPasswordAsync(
                        user, model.Token, model.Password);

                    if (resetPassword.Succeeded)
                    {
                        // Confirm email.
                        user.EmailConfirmed = true;
                        await _userHelper.UpdateUserAsync(user);

                        TempData["Message"] = "A new password has been set.";
                        return RedirectToAction(nameof(Login));
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Could not reset password.");
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    // Update user.
                    user.ChosenName = model.ChosenName;
                    user.FullName = model.FullName;
                    user.Document = model.Document;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;

                    var updateUser = await _userHelper.UpdateUserAsync(user);
                    if (updateUser.Succeeded)
                    {
                        ViewBag.UserMessage = "The account details were updated.";
                        return View(nameof(Index));
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Could not update account details.");
            return View(nameof(Index));
        }


        public IActionResult RedirectToHomePage()
        {
            return Redirect("/Home");
        }
    }
}
