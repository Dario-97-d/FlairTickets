using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace FlairTickets.Web.Helpers
{
    public interface IUserHelper
    {
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task AddUserToRoleAsync(User user, string role);
        Task<IdentityResult> ChangePasswordAsync(string email, ChangePasswordViewModel model);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> IsUserInRoleAsync(User user, string role);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<IdentityResult> UpdateUserAsync(string userName, UpdateUserViewModel model);
    }
}