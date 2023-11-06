using System;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository.Interfaces
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        Task<bool> IsFlightAfterDateTimeAsync(int flightId, DateTime dateTime);
        Task<bool> IsSeatInBoundsAsync(int flightId, int seat);
    }
}