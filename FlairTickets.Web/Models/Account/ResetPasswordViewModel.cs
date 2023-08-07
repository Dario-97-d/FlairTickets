using System.ComponentModel.DataAnnotations;

namespace FlairTickets.Web.Models.Account
{
    public class ResetPasswordViewModel
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


        public string Token { get; set; }
    }
}
