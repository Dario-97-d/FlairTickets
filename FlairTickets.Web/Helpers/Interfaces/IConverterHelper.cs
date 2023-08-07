using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Models.Entities;

namespace FlairTickets.Web.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        FlightViewModel FlightToViewModel(Flight flight);
        Task<Flight> ViewModelToFlightAsync(FlightViewModel model);
    }
}