using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository.Interfaces
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        Task CreateAsync(Flight flight);
        Task<bool> IsSeatInBoundsAsync(int flightId, int seat);
    }
}