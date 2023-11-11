using System.ComponentModel.DataAnnotations;

namespace FlairTickets.Web.Models.Account
{
    public class CreateEmployeeViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

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

        [Display(Name = "Phone number")]
        [Required]
        public string PhoneNumber { get; set; }
    }
}
