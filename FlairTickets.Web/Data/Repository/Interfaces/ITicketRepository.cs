using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Data.Repository.Interfaces
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<bool> IsSeatTakenAsync(int flightId, int seat, int ticketId);
    }
}