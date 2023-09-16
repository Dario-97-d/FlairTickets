using System;
using System.ComponentModel.DataAnnotations.Schema;
using FlairTickets.Web.Data.Repository.Interfaces;

namespace FlairTickets.Web.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime DateTime { get; set; }

        public string DateOnly => DateTime.Date.ToString();

        public string TimeOnly => DateTime.TimeOfDay.ToString();


        [ForeignKey(nameof(Origin))]
        public int OriginAirportId { get; set; }

        public Airport Origin { get; set; }


        [ForeignKey(nameof(Destination))]
        public int DestinationAirportId { get; set; }

        public Airport Destination { get; set; }


        public int AirplaneId { get; set; }

        public Airplane Airplane { get; set; }
    }
}
