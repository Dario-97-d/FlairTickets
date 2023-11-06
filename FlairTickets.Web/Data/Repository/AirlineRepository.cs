using System.Collections.Generic;
using System.Linq;
using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data.Repository
{
    public class AirlineRepository : GenericRepository<Airline>, IAirlineRepository
    {
        private readonly DataContext _context;

        public AirlineRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public IEnumerable<SelectListItem> GetComboAirlines()
        {
            return _context.Airlines
                .AsNoTracking()
                .OrderBy(a => a.Name)
                .ThenBy(a => a.IcaoCode)
                .Select(a => new SelectListItem
                {
                    Value = a.IcaoCode,
                    Text = a.Name,
                });
        }
    }
}
