using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data.Repository
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        private readonly DataContext _context;

        public FlightRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public override async Task CreateAsync(Flight flight)
        {
            await _context.Flights.AddAsync(flight);

            var navProps = new List<IEntity>
            {
                flight.Airplane,
                flight.Origin,
                flight.Destination,
            };

            navProps.ForEach(np =>
            {
                if (np != null)
                    _context.Entry(np).State = EntityState.Unchanged;
            });

            await _context.SaveChangesAsync();
        }

        public override IQueryable<Flight> GetAll()
        {
            return _context.Flights.AsNoTracking()
                .Include(f => f.Airplane)
                .Include(f => f.Origin)
                .Include(f => f.Destination);
        }

        public override async Task<Flight> GetByIdAsync(int id)
        {
            return await _context.Flights.AsNoTracking()
                .Include(f => f.Airplane)
                .Include(f => f.Origin)
                .Include(f => f.Destination)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> IsSeatInBoundsAsync(int flightId, int seat)
        {
            return await _context.Flights
                .Where(f => f.Id == flightId)
                .Select(f => seat <= f.Airplane.Seats)
                .SingleOrDefaultAsync();
        }
    }
}
