using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task CreateAsync(Ticket ticket, Flight flight);
        Task<Ticket> GetByIdWithFlightDetailsAsync(int id);
    }
}