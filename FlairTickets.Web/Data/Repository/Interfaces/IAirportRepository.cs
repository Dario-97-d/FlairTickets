using System.Collections.Generic;
using System.Threading.Tasks;
using FlairTickets.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlairTickets.Web.Data.Repository.Interfaces
{
    public interface IAirportRepository : IGenericRepository<Airport>
    {
        Task<IEnumerable<SelectListItem>> GetComboAirportsIataCodeAsync();
        Task<int> GetIdFromIataCodeAsync(string iataCode);
        Task<IEnumerable<Airport>> GetSearchAsync(Airport searchModel);
    }
}