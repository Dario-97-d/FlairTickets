using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }


        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

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

        private async Task SeedUserAsync()
        {
            string defaultEmail = "dario@e.mail";
            var user = await _userHelper.GetUserByEmailAsync(defaultEmail);

            if (user == null)
            {
                user = new User
                {
                    Email = defaultEmail,
                    UserName = defaultEmail,
                    ChosenName = "Dário",
                    FullName = "Dário Dias",
                    Document = "0123456789",
                    Address = "Road Street House"
                };

                string password = "dario@e.mail97D";

                await _userHelper.AddUserAsync(user, password);
            }
        }
    }
}
