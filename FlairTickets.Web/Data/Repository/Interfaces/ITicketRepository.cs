using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository.Interfaces
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task CreateAsync(Ticket ticket, Flight flight, User user);
        Task<Ticket> GetByIdWithFlightDetailsAsync(int id);
    }
}