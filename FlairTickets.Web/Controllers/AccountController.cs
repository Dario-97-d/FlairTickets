using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers;
using FlairTickets.Web.Models.Account;
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
            if (!User.Identity.IsAuthenticated)
                return RedirectToHomePage();

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

                    var register = await _userHelper.AddUserAsync(user, model.Password);
                    if (register.Succeeded)
                    {
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


        public IActionResult RedirectToHomePage()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
