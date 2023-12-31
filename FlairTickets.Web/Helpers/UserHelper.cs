﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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


        public async Task<IdentityResult> AddUserAsync(User user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> ChangePasswordAsync(
            User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task DeleteUserAsync(User user)
        {
            await _userManager.DeleteAsync(user);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllInRoleAsync(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }

        public async Task<Guid> GetProfilePictureGuidAsync(string userName)
        {
            return await _userManager.Users
                .Where(user => user.UserName == userName)
                .Select(user => user.ProfilePictureGuid)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetSearchInRoleAsync(User searchModel,string role)
        {
            var employees = (await _userManager.GetUsersInRoleAsync(role)).AsEnumerable();

            // Filter by Chosen name.
            if (!string.IsNullOrEmpty(searchModel.ChosenName))
                employees = employees.Where(employee =>
                    employee.ChosenName.Contains(searchModel.ChosenName));

            // Filter by Full name.
            if (!string.IsNullOrEmpty(searchModel.FullName))
                employees = employees.Where(employee =>
                    employee.FullName.Contains(searchModel.FullName));

            // Filter by Email.
            if (!string.IsNullOrEmpty(searchModel.Email))
                employees = employees.Where(employee =>
                    employee.Email.Contains(searchModel.Email));

            // Filter by Phone number.
            if (!string.IsNullOrEmpty(searchModel.PhoneNumber))
                employees = employees.Where(employee =>
                    employee.PhoneNumber.Contains(searchModel.PhoneNumber));

            return employees;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<string> GetUserChosenNameAsync(string email)
        {
            return await _userManager.Users
                .Where(u => u.Email == email)
                .Select(u => u.ChosenName)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserInRoleAsync(User user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<SignInResult> LoginAsync(User user, string password, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(User user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
