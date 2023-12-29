using System.Linq;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Models.Flight;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data
{
    public static class QueryingSelectFlights
    {
        public static async Task<DisplayFlightViewModel>
            SelectSingleDisplayViewModelAsync(IQueryable<Flight> iQueryableFlights)
        {
            return await iQueryableFlights.Select(flight => new DisplayFlightViewModel
            {
                Id = flight.Id,
                DateTime = string.Format("{0:d} {0:HH:mm}", flight.DateTime),
                Number = flight.Number,
                OriginAirport = flight.Origin.ComboName,
                OriginCountryCode2Letters = flight.Origin.CountryCode2Letters,
                DestinationAirport = flight.Destination.ComboName,
                DestinationCountryCode2Letters = flight.Destination.CountryCode2Letters,
                Airplane = flight.Airplane.ComboName,
            }).SingleOrDefaultAsync();
        }
    }
}
