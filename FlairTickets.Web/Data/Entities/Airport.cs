using System.ComponentModel.DataAnnotations;
using FlairTickets.Web.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlairTickets.Web.Data.Entities
{
    [Index(nameof(IataCode), IsUnique = true)]
    public class Airport : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string IataCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [MaxLength(2)]
        [MinLength(2)]
        public string CountryCode2Letters { get; set; }

        public string ComboName => $"{City} ({IataCode})";
    }
}
