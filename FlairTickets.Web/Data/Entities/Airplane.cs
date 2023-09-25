using System.ComponentModel.DataAnnotations;
using FlairTickets.Web.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Airplane : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Seats { get; set; }

        public string ComboName => $"{Model} ({Seats}) - {Name}";
    }
}
