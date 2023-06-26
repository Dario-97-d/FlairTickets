using System.ComponentModel.DataAnnotations;

namespace FlairTickets.Web.Models.Account
{
    public class RegisterViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }


        [Compare("Password")]
        [Display(Name = "Confirm password")]
        [Required]
        public string ConfirmPassword { get; set; }



        [Display(Name = "Chosen name")]
        [Required]
        public string ChosenName { get; set; }


        [Display(Name = "Full name")]
        [Required]
        public string FullName { get; set; }


        [Required]
        public string Document { get; set; }


        [Required]
        public string Address { get; set; }


        [Display(Name = "Phone number")]
        [Required]
        public string PhoneNumber { get; set; }
    }
}
