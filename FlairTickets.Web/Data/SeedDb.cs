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
            await SeedUserAsync();

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
                    new Airplane { Model = "A320", Name = "Rambo", Seats = 150 },
                    new Airplane { Model = "747-400", Name = "Jumbo", Seats = 416 },
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
                        Country = "Portugal"
                    },
                    new Airport
                    {
                        IataCode = "LIS",
                        Name = "Humberto Delgado",
                        City = "Lisboa",
                        Country = "Portugal"
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

        private async Task SeedUserAsync()
        {
            string email = _configuration["SeedDb:DefaultUser:Email"];
            var user = await _userHelper.GetUserByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    UserName = email,
                    ChosenName = _configuration["SeedDb:DefaultUser:ChosenName"],
                    FullName = _configuration["SeedDb:DefaultUser:FullName"],
                    Document = _configuration["SeedDb:DefaultUser:Document"],
                    Address = _configuration["SeedDb:DefaultUser:Address"]
                };

                string password = email;

                await _userHelper.AddUserAsync(user, password);

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }
            else
            {
                if (!await _userHelper.IsUserInRoleAsync(user, "Admin"))
                {
                    await _userHelper.AddUserToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
