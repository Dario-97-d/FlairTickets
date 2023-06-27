using System.ComponentModel.DataAnnotations;

namespace FlairTickets.Web.Models.Account
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Current password")]
        [Required]
        public string OldPassword { get; set; }

        [Display(Name = "New password")]
        [Required]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        [Display(Name = "Confirm password")]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
