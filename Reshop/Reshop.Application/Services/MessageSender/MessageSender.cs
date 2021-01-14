using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.IMessageSender;

namespace Reshop.Application.Services.MessageSender
{
    public class MessageSender : IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            using var client = new SmtpClient();

            var credentials = new NetworkCredential()
            {
                UserName = "esmaeilemami84", // without @gmail.com
                Password = "Hunter@1234"
            };

            client.Credentials = credentials;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;

            using var emailMessage = new MailMessage()
            {
                To = { new MailAddress(toEmail) },
                From = new MailAddress("esmaeilemami84@gmail.com"), // with @gmail.com
                Subject = subject,
                Body = message,
                IsBodyHtml = isMessageHtml
            };

            client.Send(emailMessage);

            return Task.CompletedTask;

        }
    }
}