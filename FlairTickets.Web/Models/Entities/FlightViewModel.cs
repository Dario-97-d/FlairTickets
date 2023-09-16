using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlairTickets.Web.Models.Entities
{
    public class FlightViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime DateTime { get; set; }

        [Range(1, int.MaxValue)]
        public int OriginAirportId { get; set; }

        [Range(1, int.MaxValue)]
        public int DestinationAirportId { get; set; }

        [Range(1, int.MaxValue)]
        public int AirplaneId { get; set; }


        public IEnumerable<SelectListItem> ComboAirports { get; set; }

        public IEnumerable<SelectListItem> ComboAirplanes { get; set; }
    }
}
