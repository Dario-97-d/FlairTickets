using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers;
using FlairTickets.Web.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlairTickets.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountController(IUserHelper userHelper)
        {
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
                var changePassword = await _userHelper.ChangePasswordAsync(
                    User.Identity.Name,
                    model);

                if (changePassword.Succeeded)
                {
                    ViewBag.UserMessage = "Password updated!";
                    return View();
                }
            }

            ModelState.AddModelError(string.Empty, "Could not update password.");
            return View();
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
            if (User.Identity.IsAuthenticated)
                return RedirectToHomePage();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToHomePage();

            if (ModelState.IsValid)
            {
                var login = await _userHelper.LoginAsync(model);
                if (login.Succeeded)
                {
                    if (Request.Query.ContainsKey("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"]);
                    }
                    else return RedirectToHomePage();
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
            if (User.Identity.IsAuthenticated)
                return RedirectToHomePage();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToHomePage();

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
                        await _userHelper.AddUserToRoleAsync(user, "Customer");

                        var loginViewModel = new LoginViewModel
                        {
                            Email = model.Email,
                            Password = model.Password,
                            RememberMe = false
                        };

                        return await Login(loginViewModel);
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Could not sign up.");
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updateUser = await _userHelper.UpdateUserAsync(User.Identity.Name, model);
                if (updateUser.Succeeded)
                {
                    ViewBag.UserMessage = "The account details were updated.";
                    return View(nameof(Index));
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
