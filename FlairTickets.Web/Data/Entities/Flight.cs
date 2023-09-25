using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlairTickets.Web.Data.Repository.Interfaces;

namespace FlairTickets.Web.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public string DateOnly => DateTime.Date.ToString();

        public string TimeOnly => DateTime.TimeOfDay.ToString();


        [ForeignKey(nameof(Origin))]
        [Required]
        public int OriginAirportId { get; set; }

        public Airport Origin { get; set; }


        [ForeignKey(nameof(Destination))]
        [Required]
        public int DestinationAirportId { get; set; }

        public Airport Destination { get; set; }


        [Required]
        public int AirplaneId { get; set; }

        public Airplane Airplane { get; set; }
    }
}
