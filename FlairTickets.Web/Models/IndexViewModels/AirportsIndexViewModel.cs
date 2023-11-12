using System.Collections.Generic;
using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Models.IndexViewModels
{
    public class AirportsIndexViewModel
    {
        public Airport SearchModel { get; set; }

        public IEnumerable<Airport> Airports { get; set; }
    }
}
