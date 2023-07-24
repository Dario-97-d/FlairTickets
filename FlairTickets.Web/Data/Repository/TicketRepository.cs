using System.Linq;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
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


        public async Task CreateAsync(Ticket ticket, Flight flight)
        {
            await _context.AddAsync(ticket);

            ticket.Flight = flight;

            await _context.SaveChangesAsync();
        }

        public override IQueryable<Ticket> GetAll()
        {
            return _context.Tickets.AsNoTracking()
                .Include(t => t.Flight).ThenInclude(f => f.Origin)
                .Include(t => t.Flight).ThenInclude(f => f.Destination);
        }

        public override async Task<Ticket> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Include(t => t.Flight)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket> GetByIdWithFlightDetailsAsync(int id)
        {
            return await _context.Tickets.AsNoTracking()
                .Include(t => t.Flight).ThenInclude(f => f.Origin)
                .Include(t => t.Flight).ThenInclude(f => f.Destination)
                .Include(t => t.Flight).ThenInclude(f => f.Airplane)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
