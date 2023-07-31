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

        public async Task AddUserToRoleAsync(User user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string email, ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return await _userManager.ChangePasswordAsync(
                    user,
                    model.OldPassword,
                    model.NewPassword);
            }

            return IdentityResult.Failed();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                return await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    false);
            }
            
            return SignInResult.Failed;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(string userName, UpdateUserViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            if (user != null)
            {
                user.ChosenName = model.ChosenName;
                user.FullName = model.FullName;
                user.Document = model.Document;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;

                return await _userManager.UpdateAsync(user);
            }

            return IdentityResult.Failed();
        }
    }
}
