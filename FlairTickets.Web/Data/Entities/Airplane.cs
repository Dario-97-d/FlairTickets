using System;
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

        [Required]
        public Guid PhotoGuid { get; set; }

        public string PhotoUrl => PhotoGuid != Guid.Empty
            ? "https://hybriotheca.blob.core.windows.net/airplanes/" + PhotoGuid.ToString()
            : null;


        public string ComboName => $"{Model} ({Seats}) - {Name}";
    }
}
