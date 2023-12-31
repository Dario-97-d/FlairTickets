﻿using System;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FlairTickets.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserHelper _userHelper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedDb(
            DataContext context,
            IConfiguration configuration,
            IUserHelper userHelper,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _configuration = configuration;
            _userHelper = userHelper;
            _roleManager = roleManager;
        }


        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await SeedRolesAsync();
            await SeedUsersAsync();

            await SeedAirplanesAsync();
            await SeedAirportsAsync();

            await _context.SaveChangesAsync();
        }


        private async Task SeedAirplanesAsync()
        {
            if (!await _context.Airplanes.AnyAsync())
            {
                var airplanes = new Airplane[]
                {
                    new Airplane
                    {
                        Model = "A320",
                        Name = "Rambo",
                        Seats = 150,
                        PhotoGuid = Guid.Parse("d07d90c1-b8d6-4c8a-9fbe-b15550ef18b6"),
                    },
                    new Airplane
                    {
                        Model = "747-400",
                        Name = "Jumbo",
                        Seats = 416,
                        PhotoGuid = Guid.Parse("c34b4081-c219-4c19-94ee-1b4bc01bcf19"),
                    },
                };

                _context.Airplanes.AddRange(airplanes);
            }
        }

        private async Task SeedAirportsAsync()
        {
            if (!await _context.Airports.AnyAsync())
            {
                var airports = new Airport[]
                {
                    new Airport {
                        IataCode = "OPO",
                        Name = "Francisco Sá Carneiro",
                        City = "Porto",
                        Country = "Portugal",
                        CountryCode2Letters = "PT",
                    },
                    new Airport
                    {
                        IataCode = "LIS",
                        Name = "Humberto Delgado",
                        City = "Lisboa",
                        Country = "Portugal",
                        CountryCode2Letters = "PT",
                    }
                };

                _context.Airports.AddRange(airports);
            }
        }

        private async Task SeedRolesAsync()
        {
            string[] roles = _configuration["SeedDb:Roles"].Split(',');

            foreach (string role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            var usersNames = _configuration["SeedDb:Users:AllUsers"].Split(',');

            foreach (var userName in usersNames)
            {
                string email = _configuration[$"SeedDb:Users:{userName}:Email"];

                var user = await _userHelper.GetUserByEmailAsync(email);

                if (user == null)
                {
                    user = new User
                    {
                        Email = email,
                        UserName = email,
                        ChosenName = _configuration[$"SeedDb:Users:{userName}:ChosenName"],
                        FullName = _configuration[$"SeedDb:Users:{userName}:FullName"],
                        Document = _configuration[$"SeedDb:Users:{userName}:Document"],
                        Address = _configuration[$"SeedDb:Users:{userName}:Address"],
                        EmailConfirmed = true,
                    };

                    string password = _configuration[$"SeedDb:Users:{userName}:Password"];
                    await _userHelper.AddUserAsync(user, password);
                }

                string role = _configuration[$"SeedDb:Users:{userName}:Role"];

                if (!await _userHelper.IsUserInRoleAsync(user, role))
                {
                    await _userHelper.AddUserToRoleAsync(user, role);
                }
            }
        }
    }
}
