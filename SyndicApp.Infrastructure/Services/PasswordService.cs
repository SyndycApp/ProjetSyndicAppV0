// Fichier : SyndicApp.Infrastructure/Services/PasswordService.cs
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SyndicApp.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SyndicApp.Infrastructure.Identity;



namespace SyndicApp.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _email;
        private readonly IDataProtector _protector;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _cfg;


        public PasswordService(ApplicationDbContext db, IEmailSender email, IDataProtectionProvider provider, IServiceProvider serviceProvider, IConfiguration cfg)
        {
            _db = db;
            _email = email;
            _protector = provider.CreateProtector("PasswordResetTokens");
            _serviceProvider = serviceProvider;
            _cfg = cfg;
        }

        private static string Generate6Digits()
    => RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");


        public async Task<bool> SendResetCodeAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user is null || string.IsNullOrWhiteSpace(user.Email)) return false;

            var code = Generate6Digits();

            user.PasswordResetCode = code;
            user.PasswordResetCodeExpires = DateTime.UtcNow.AddMinutes(10);
            user.PasswordResetCodeAttempts = 0;
            await _db.SaveChangesAsync();

            var html = $@"
<html><body style='font-family:Arial'>
  <h2>Code de réinitialisation</h2>
  <p>Votre code :</p>
  <h1 style='text-align:center'>{code}</h1>
  <p>Le code expire dans <b>10 minutes</b>.</p>
  <p style='color:#888'>— L'équipe SyndicApp</p>
</body></html>";

            await _email.SendAsync(user.Email, "Code de réinitialisation", html);
            return true;
        }

        public async Task<bool> VerifyResetCodeAsync(string email, string code)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code)) return false;

            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user is null) return false;

            if (user.PasswordResetCodeAttempts >= 5) return false;

            bool ok = user.PasswordResetCode == code && user.PasswordResetCodeExpires > DateTime.UtcNow;

            user.PasswordResetCodeAttempts += 1;
            await _db.SaveChangesAsync();

            return ok;
        }

        public async Task<bool> ResetWithCodeAsync(string email, string code, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code)) return false;
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword != confirmPassword) return false;

            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user is null) return false;

            if (user.PasswordResetCode != code ||
                user.PasswordResetCodeExpires is null ||
                user.PasswordResetCodeExpires <= DateTime.UtcNow)
                return false;

            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Si aucun mot de passe n'existait, RemovePasswordAsync peut échouer : on tente Add d'abord
            var hasPwd = await userManager.HasPasswordAsync(user);
            IdentityResult addResult;

            if (hasPwd)
            {
                var remove = await userManager.RemovePasswordAsync(user);
                if (!remove.Succeeded) return false;
                addResult = await userManager.AddPasswordAsync(user, newPassword);
            }
            else
            {
                addResult = await userManager.AddPasswordAsync(user, newPassword);
            }

            if (!addResult.Succeeded) return false;

            // purge OTP
            user.PasswordResetCode = null;
            user.PasswordResetCodeExpires = null;
            user.PasswordResetCodeAttempts = 0;
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> GenerateResetTokenAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("L'adresse email ne peut pas être vide ou nulle.", nameof(email));

            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user is null) return false;

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new InvalidOperationException("L'utilisateur n'a pas d'adresse email valide.");

            // 🔐 Génération d’un token sécurisé
            var raw = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var token = _protector.Protect($"{raw}|{DateTime.UtcNow:O}");

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);
            await _db.SaveChangesAsync();

            // 📱 Deep link mobile (ouvre l’app directement)
            var appLink = $"syndicapp://app/resetpassword?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email!)}";

            // 🌐 Lien web de secours si l’app n’est pas installée
            var webFallback = _cfg["Frontend:ResetPasswordFallbackUrl"] ?? "https://localhost:7263/resetpassword";

            // 💌 Corps HTML de l’e-mail
            var htmlBody = $@"
<html>
  <body style=""font-family:Arial,sans-serif;background-color:#f4f4f4;padding:20px;"">
    <div style=""max-width:600px;margin:auto;background-color:#ffffff;padding:30px;border-radius:10px;box-shadow:0 2px 4px rgba(0,0,0,0.1);"">
      <h2 style=""color:#2c3e50;text-align:center;"">Réinitialisation de votre mot de passe</h2>
      <p>Bonjour <strong>{user.FullName ?? user.Email}</strong>,</p>
      <p>Nous avons reçu une demande de réinitialisation de votre mot de passe.</p>
      <p style=""text-align:center;margin:25px 0;"">
        <a href=""{appLink}"" style=""display:inline-block;background-color:#3498db;color:#fff;
            text-decoration:none;padding:12px 28px;border-radius:6px;font-weight:bold;"">
          Ouvrir dans l'application
        </a>
      </p>
      <p style=""text-align:center;margin-top:15px;"">
        Si l'application n'est pas installée, cliquez ici :
        <a href=""{webFallback}?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email!)}"">
          Réinitialiser sur le site web
        </a>
      </p>
      <p style=""font-size:13px;color:#7f8c8d;margin-top:30px;"">
        Ce lien est valide pendant <strong>1 heure</strong>.<br/>
        Si vous n'avez pas demandé cette opération, ignorez simplement ce message.
      </p>
      <hr style=""margin-top:30px;border:none;border-top:1px solid #ddd;""/>
      <p style=""font-size:12px;color:#aaa;text-align:center;"">
        — L’équipe <strong>SyndicApp</strong>
      </p>
    </div>
  </body>
</html>";

            // ✉️ Envoi de l'e-mail
            await _email.SendAsync(
                user.Email!,
                "Réinitialisation de votre mot de passe",
                htmlBody
            );

            return true;
        }


        public async Task<bool> ResetPasswordAsync(ResetPasswordDto model)
        {
            if (model.NewPassword != model.ConfirmPassword) return false;

            var user = await _db.Users.SingleOrDefaultAsync(u =>
                u.PasswordResetToken == model.Token &&
                u.PasswordResetTokenExpires > DateTime.UtcNow);

            if (user is null) return false;

            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Supprimer l'ancien mot de passe (si existant)
            var removeResult = await userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded) return false;

            // Ajouter le nouveau mot de passe
            var addResult = await userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addResult.Succeeded) return false;

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;
            await _db.SaveChangesAsync();

            return true;
        }

    }
}
