using Microsoft.AspNetCore.Http;

namespace FlairTickets.Web.Models.Account
{
    public class ChangeProfilePictureViewModel
    {
        public IFormFile? PhotoFile { get; set; }

        public bool HasPhoto { get; set; }

        public string PhotoFullPath { get; set; }

        public bool DeletePhoto { get; set; }
    }
}
