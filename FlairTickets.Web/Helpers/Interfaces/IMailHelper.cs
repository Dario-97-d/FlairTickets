using FlairTickets.Web.Data.Entities;

namespace FlairTickets.Web.Helpers.Interfaces
{
    public interface IMailHelper
    {
        bool SendConfirmationEmail(User user, string tokenUrl);
        bool SendEmail(string emailTo, string subject, string body);
        bool SendPasswordResetEmail(User user, string tokenUrl);
    }
}