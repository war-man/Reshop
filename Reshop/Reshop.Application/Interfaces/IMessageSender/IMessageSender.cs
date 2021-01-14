using System.Threading.Tasks;

namespace Reshop.Application.Interfaces.IMessageSender
{
    public interface IMessageSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false);
    }
}