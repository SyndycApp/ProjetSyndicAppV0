using SyndicApp.Application.Interfaces;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            // TODO: Implémenter SMTP réel ou service tiers ici
            System.Console.WriteLine($"Envoi email à {to} - Sujet: {subject}\nCorps:\n{body}");
            return Task.CompletedTask;
        }
    }
}