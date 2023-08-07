using System.ComponentModel.DataAnnotations;

namespace FlairTickets.Web.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
