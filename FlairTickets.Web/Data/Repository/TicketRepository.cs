using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly DataContext _context;

        public TicketRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public async Task<bool> IsSeatTakenAsync(int flightId, int seat, int ticketId)
        {
            return await _context.Tickets.AnyAsync(
                t => t.Flight.Id == flightId && t.Seat == seat && t.Id != ticketId);
        }
    }
}
