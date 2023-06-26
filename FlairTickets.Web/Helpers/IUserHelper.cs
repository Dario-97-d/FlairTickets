using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace FlairTickets.Web.Helpers
{
    public interface IUserHelper
    {
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}