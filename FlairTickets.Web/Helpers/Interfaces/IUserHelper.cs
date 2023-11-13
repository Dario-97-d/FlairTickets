using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace FlairTickets.Web.Helpers.Interfaces
{
    public interface IUserHelper
    {
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<IdentityResult> AddUserAsync(User user);
        Task AddUserToRoleAsync(User user, string role);
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task DeleteUserAsync(User user);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IEnumerable<User>> GetAllInRoleAsync(string role);
        Task<Guid> GetProfilePictureGuidAsync(string userName);
        Task<IEnumerable<User>> GetSearchInRoleAsync(User searchModel, string role);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(string id);
        Task<string> GetUserChosenNameAsync(string id);
        Task<bool> IsUserInRoleAsync(User user, string role);
        Task<SignInResult> LoginAsync(User user, string password, bool rememberMe);
        Task LogoutAsync();
        Task<IdentityResult> RemoveFromRoleAsync(User user, string role);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
    }
}