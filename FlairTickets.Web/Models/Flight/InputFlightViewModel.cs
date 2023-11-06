using System;
using System.ComponentModel.DataAnnotations;

namespace FlairTickets.Web.Models.Flight
{
    public class InputFlightViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public string OriginAirportIataCode { get; set; }

        [Required]
        public string DestinationAirportIataCode { get; set; }

        [Required]
        public int AirplaneId { get; set; }
    }
}
