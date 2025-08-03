// Fichier : SyndicApp.Infrastructure/Services/PasswordService.cs
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Application.Interfaces;
using System;
using System.Threading.Tasks;
using BCrypt.Net;
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


        public PasswordService(ApplicationDbContext db, IEmailSender email, IDataProtectionProvider provider, IServiceProvider serviceProvider)
        {
            _db = db;
            _email = email;
            _protector = provider.CreateProtector("PasswordResetTokens");
            _serviceProvider = serviceProvider;
        }


        public async Task<bool> GenerateResetTokenAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("L'adresse email ne peut pas être vide ou nulle.", nameof(email));

            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user is null) return false;

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new InvalidOperationException("L'utilisateur n'a pas d'adresse email valide.");

            var raw = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var token = _protector.Protect($"{raw}|{DateTime.UtcNow:O}");

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);
            await _db.SaveChangesAsync();

            var url = $"https://localhost:7263/resetpassword?token={Uri.EscapeDataString(token)}";


            await _email.SendAsync(user.Email, "Réinitialisation de votre mot de passe",
$@"
<html>
  <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;"">
    <div style=""max-width: 600px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);"">
      <h2 style=""color: #2c3e50;"">Réinitialisation de votre mot de passe</h2>
      <p>Bonjour,</p>
      <p>Nous avons reçu une demande de réinitialisation de votre mot de passe. Si vous êtes à l'origine de cette demande, cliquez sur le bouton ci-dessous pour continuer :</p>
      <p style=""text-align: center;"">
        <a href=""{url}"" style=""display: inline-block; background-color: #3498db; color: #fff; text-decoration: none; padding: 12px 24px; border-radius: 5px; font-weight: bold;"">
          Réinitialiser le mot de passe
        </a>
      </p>
      <p>Ce lien est valide pendant 1 heure. Si vous n'avez pas demandé cette opération, vous pouvez ignorer ce message.</p>
      <p style=""font-size: 12px; color: #888888;"">— L’équipe SyndicApp</p>
    </div>
  </body>
</html>
");


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
