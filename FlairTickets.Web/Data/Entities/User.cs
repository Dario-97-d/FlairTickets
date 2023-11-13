using System;
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

        [Display(Name = "Active?")]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }

        [Display(Name = "Phone number")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        public Guid ProfilePictureGuid { get; set; }
    }
}
