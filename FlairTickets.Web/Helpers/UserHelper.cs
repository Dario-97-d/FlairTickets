using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace FlairTickets.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public UserHelper(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            var user = await GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                return await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    false);
            }
            else return SignInResult.Failed;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
