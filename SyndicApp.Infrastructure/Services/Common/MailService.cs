using SyndicApp.Application.Interfaces.Common;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

public class MailService : IMailService
{
    private readonly IConfiguration _config;

    public MailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task EnvoyerAsync(string email, string sujet, string contenu)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidOperationException("Email destinataire invalide");

        var smtp = _config.GetSection("Smtp");

        var client = new SmtpClient
        {
            Host = smtp["Host"],
            Port = int.Parse(smtp["Port"]!),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                smtp["User"],
                smtp["Pass"]
            )
        };

        var message = new MailMessage
        {
            From = new MailAddress(
                smtp["FromEmail"]!,
                smtp["FromName"]!
            ),
            Subject = sujet,
            Body = contenu,
            IsBodyHtml = true
        };

        message.To.Add(email);

        await client.SendMailAsync(message);
    }
}
