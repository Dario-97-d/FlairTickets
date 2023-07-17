using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlairTickets.Web.Models.Entities
{
    public class FlightViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        public int OriginAirportId { get; set; }

        public int DestinationAirportId { get; set; }

        public int AirplaneId { get; set; }


        public IEnumerable<SelectListItem> Airports { get; set; }

        public IEnumerable<SelectListItem> Airplanes { get; set; }
    }
}
