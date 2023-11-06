using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data.Repository
{
    public class AirportRepository : GenericRepository<Airport>, IAirportRepository
    {
        private readonly DataContext _context;

        public AirportRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<SelectListItem>> GetComboAirportsIataCodeAsync()
        {
            return await _context.Airports
                .AsNoTracking()
                .OrderBy(a => a.City)
                .ThenBy(a => a.IataCode)
                .Select(a => new SelectListItem
                {
                    Value = a.IataCode.ToString(),
                    Text = a.ComboName,
                }).ToListAsync();
        }

        public async Task<int> GetIdFromIataCodeAsync(string iataCode)
        {
            return await _context.Airports
                .Where(a => a.IataCode == iataCode)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();
        }
    }
}
