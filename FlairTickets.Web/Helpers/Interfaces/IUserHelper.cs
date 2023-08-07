using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace FlairTickets.Web.Helpers.Interfaces
{
    public interface IUserHelper
    {
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task AddUserToRoleAsync(User user, string role);
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> IsUserInRoleAsync(User user, string role);
        Task<SignInResult> LoginAsync(User user, string password, bool rememberMe);
        Task LogoutAsync();
        Task<IdentityResult> UpdateUserAsync(User user);
    }
}