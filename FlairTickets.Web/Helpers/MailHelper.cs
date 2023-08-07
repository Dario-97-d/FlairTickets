using FlairTickets.Web.Data.Entities;
using FlairTickets.Web.Helpers.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace FlairTickets.Web.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendConfirmationEmail(User user, string tokenUrl)
        {
            string emailBody = "<h2>Email confirmation</h2>" +
                $"<p>Click <a href=\"{tokenUrl}\"><u>here</u></a> to activate account.</p>";

            try
            {
                return SendEmail(user.Email, "Email confirmation", emailBody);
            }
            catch { return false; }
        }

        public bool SendEmail(string emailTo, string subject, string body)
        {
            var nameFrom = _configuration["Mail:NameFrom"];
            var emailFrom = _configuration["Mail:EmailFrom"];
            var smtp = _configuration["Mail:Smtp"];
            var port = _configuration["Mail:Port"];
            var password = _configuration["Mail:Password"];

            var length = emailTo.IndexOf("@");
            var nameTo = emailTo[0..length];

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(nameFrom, emailFrom));
            message.To.Add(new MailboxAddress(nameTo, emailTo));

            message.Subject = subject;

            message.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();

            try
            {
                using var client = new SmtpClient();

                client.Connect(smtp, int.Parse(port), false);
                client.Authenticate(emailFrom, password);
                client.Send(message);
                client.Disconnect(true);

                return true;
            }
            catch { return false; }
        }

        public bool SendPasswordResetEmail(User user, string tokenUrl)
        {
            string emailBody = "<h2>Password reset</h2>" +
                $"<p>Click <a href=\"{tokenUrl}\"><u>here</u></a> to reset password.</p>";

            try
            {
                return SendEmail(user.Email, "Password reset", emailBody);
            }
            catch { return false; }
        }
    }
}
