using System.Linq;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository.Interfaces
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task CreateAsync(Ticket ticket, Flight flight, User user);
        IQueryable<Ticket> GetAllOfUser(User user);
        Task<Ticket> GetByIdWithFlightDetailsAsync(int id);
        Task<bool> IsSeatTakenAsync(int flightId, int seat, int ticketId);
    }
}