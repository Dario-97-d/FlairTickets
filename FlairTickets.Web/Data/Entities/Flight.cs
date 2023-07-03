using System;
using FlairTickets.Web.Data.Repository;

namespace FlairTickets.Web.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public Airport Origin { get; set; }

        public Airport Destination { get; set; }

        public Airplane Airplane { get; set; }
    }
}
