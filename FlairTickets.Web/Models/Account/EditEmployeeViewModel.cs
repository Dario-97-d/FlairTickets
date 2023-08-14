using System.ComponentModel.DataAnnotations;

namespace FlairTickets.Web.Models.Account
{
    public class EditEmployeeViewModel
    {
        [Required]
        public string Id { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Name")]
        public string ChosenName { get; set; }
    }
}
