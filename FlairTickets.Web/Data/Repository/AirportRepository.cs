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

        public async Task<IEnumerable<Airport>> GetSearchAsync(Airport searchModel)
        {
            var airports = _context.Airports.AsNoTracking();

            // Filter by IATA Code.
            if (!string.IsNullOrEmpty(searchModel.IataCode))
                airports = airports.Where(airport => airport.IataCode.Contains(searchModel.IataCode));

            // Filter by Name.
            if (!string.IsNullOrEmpty(searchModel.Name))
                airports = airports.Where(airport => airport.Name.Contains(searchModel.Name));

            // Filter by City.
            if (!string.IsNullOrEmpty(searchModel.City))
                airports = airports.Where(airport => airport.City.Contains(searchModel.City));

            // Filter by Country.
            if (!string.IsNullOrEmpty(searchModel.Country))
                airports = airports
                    .Where(airport =>
                        airport.Country.Contains(searchModel.Country)
                        || airport.CountryCode2Letters == searchModel.Country.ToUpper());

            return await airports.ToListAsync();
        }
    }
}
