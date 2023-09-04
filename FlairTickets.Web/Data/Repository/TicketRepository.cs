using System.Linq;
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


        public async Task CreateAsync(Ticket ticket, Flight flight, User user)
        {
            await _context.AddAsync(ticket);

            ticket.Flight = flight;
            ticket.User = user;

            await _context.SaveChangesAsync();
        }

        public override IQueryable<Ticket> GetAll()
        {
            return _context.Tickets.AsNoTracking()
                .Include(t => t.Flight).ThenInclude(f => f.Origin)
                .Include(t => t.Flight).ThenInclude(f => f.Destination);
        }

        public IQueryable<Ticket> GetAllOfUser(User user)
        {
            return _context.Tickets.AsNoTracking()
                .Where(t => t.User == user)
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

        public async Task<bool> IsSeatTakenAsync(int flightId, int seat)
        {
            return await _context.Tickets.AnyAsync(t => t.Flight.Id == flightId && t.Seat == seat);
        }

        public async Task<bool> IsSeatInBoundsAsync(int flightId, int seat)
        {
            var flight = await _context.Flights.AsNoTracking()
                .Include(f => f.Airplane)
                .FirstOrDefaultAsync(f => f.Id == flightId);
            
            return flight.Airplane.Seats >= seat;
        }
    }
}
