using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlairTickets.Web.Data.Repository.Interfaces
{
    public interface IAirlineRepository
    {
        IEnumerable<SelectListItem> GetComboAirlines();
    }
}