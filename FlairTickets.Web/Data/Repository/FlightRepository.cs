using System;
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


        public async Task<bool> IsFlightAfterDateTimeAsync(int flightId, DateTime dateTime)
        {
            return await _context.Flights
                .Where(f => f.Id == flightId)
                .Select(f => EF.Functions.DateDiffSecond(f.DateTime, dateTime) > 0)
                .SingleOrDefaultAsync();
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
