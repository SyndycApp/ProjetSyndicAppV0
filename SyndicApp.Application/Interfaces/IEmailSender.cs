using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces
{
    public interface IEmailSender
    {
         Task SendAsync(string to, string subject, string htmlBody);
    }
}
