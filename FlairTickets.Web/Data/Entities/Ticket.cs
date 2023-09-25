using System.ComponentModel.DataAnnotations;
using FlairTickets.Web.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data.Entities
{
    [Index("FlightId", "Seat", IsUnique = true)]
    public class Ticket : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string UserId { get; set; }

        public User User { get; set; }


        [Required]
        public int FlightId { get; set; }

        public Flight Flight { get; set; }


        [Required]
        public int Seat { get; set; }
    }
}
