using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Infrastructure;

namespace SyndicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PasswordController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
                return BadRequest("Email requis.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return Ok("Si cette adresse est valide, un email vous a été envoyé."); // Ne révèle pas l'existence

            var token = GenerateSecureToken();
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);

            await _context.SaveChangesAsync();

            var frontendBaseUrl = _configuration["FrontendBaseUrl"];
            if (string.IsNullOrWhiteSpace(frontendBaseUrl))
                return StatusCode(500, "FrontendBaseUrl manquant dans la configuration.");

            var resetUrl = $"{frontendBaseUrl}/resetpassword?token={Uri.EscapeDataString(token)}";

            await SendResetPasswordEmail(user.Email, resetUrl);

            return Ok("Si cette adresse est valide, un email vous a été envoyé.");
        }

        private string GenerateSecureToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes)
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "");
        }

        private async Task SendResetPasswordEmail(string? email, string resetUrl)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email), "L'adresse email ne peut pas être vide.");

            var smtpHost = _configuration["Smtp:Host"];
            var smtpPortStr = _configuration["Smtp:Port"];
            var smtpUser = _configuration["Smtp:User"];
            var smtpPass = _configuration["Smtp:Pass"];
            var fromEmail = _configuration["Smtp:FromEmail"];
            var fromName = _configuration["Smtp:FromName"];

            if (!int.TryParse(smtpPortStr, out int smtpPort))
                throw new InvalidOperationException("Port SMTP invalide ou manquant dans la configuration.");

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail ?? throw new ArgumentNullException(nameof(fromEmail)), fromName ?? "SyndicApp"),
                Subject = "Réinitialisation de votre mot de passe",
                IsBodyHtml = true,
                Body = $@"
                    <p>Bonjour,</p>
                    <p>Vous avez demandé à réinitialiser votre mot de passe.</p>
                    <p>Veuillez cliquer sur ce lien pour définir un nouveau mot de passe :</p>
                    <p><a href='{resetUrl}'>Réinitialiser mon mot de passe</a></p>
                    <p>Si vous n'êtes pas à l'origine de cette demande, ignorez ce message.</p>
                    <p>Cordialement,<br/>L'équipe SyndicApp</p>"
            };

            message.To.Add(email);

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}
