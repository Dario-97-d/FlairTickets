using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FlairTickets.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Chosen name")]
        [MaxLength(15)]
        [Required]
        public string ChosenName { get; set; }

        [Display(Name = "Full name")]
        [MaxLength(60)]
        [Required]
        public string FullName { get; set; }
        
        [MaxLength(15)]
        [Required]
        public string Document { get; set; }

        [MaxLength(120)]
        [Required]
        public string Address { get; set; }
    }
}
