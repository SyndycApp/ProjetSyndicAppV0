using SyndicApp.Application.Interfaces;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendAsync(string to, string subject, string htmlBody)
        {
            // TODO: Implémenter SMTP réel ou service tiers ici
            System.Console.WriteLine($"Envoi email à {to} - Sujet: {subject}\nCorps:\n{htmlBody}");
            return Task.CompletedTask;
        }
    }
}