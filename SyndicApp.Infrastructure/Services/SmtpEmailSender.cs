using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using SyndicApp.Application.Interfaces;

namespace SyndicApp.Infrastructure.Services
{
    public sealed class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _cfg;

        public SmtpEmailSender(IConfiguration cfg) => _cfg = cfg;

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var host = _cfg["Smtp:Host"]!;
            var port = int.Parse(_cfg["Smtp:Port"] ?? "587");
            var user = _cfg["Smtp:User"];
            var pass = _cfg["Smtp:Pass"];
            var from = _cfg["Smtp:FromEmail"]!;
            var name = _cfg["Smtp:FromName"] ?? "SyndicApp";

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(user, pass)
            };

            var msg = new MailMessage(from, to, subject, htmlBody)
            {
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };

            await client.SendMailAsync(msg);
        }
    }
}
