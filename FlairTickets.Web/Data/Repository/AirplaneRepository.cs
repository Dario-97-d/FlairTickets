using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data.Repository
{
    public class AirplaneRepository : GenericRepository<Airplane>, IAirplaneRepository
    {
        private readonly DataContext _context;

        public AirplaneRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Airplane>> GetSearchAsync(Airplane searchModel)
        {
            var airplanes = _context.Airplanes.AsNoTracking();

            // Filter by Model.
            if (!string.IsNullOrEmpty(searchModel.Model))
                airplanes = airplanes.Where(airplane => airplane.Model.Contains(searchModel.Model));

            // Filter by Name.
            if (!string.IsNullOrEmpty(searchModel.Name))
                airplanes = airplanes.Where(airplane => airplane.Name.Contains(searchModel.Name));

            return await airplanes.ToListAsync();
        }
    }
}
