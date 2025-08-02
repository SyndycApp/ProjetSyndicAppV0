// Fichier : SyndicApp.Infrastructure/Services/PasswordService.cs
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Application.Interfaces;
using System;
using System.Threading.Tasks;
using BCrypt.Net;


namespace SyndicApp.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _email;
        private readonly IDataProtector _protector;

        public PasswordService(ApplicationDbContext db, IEmailSender email, IDataProtectionProvider provider)
        {
            _db = db;
            _email = email;
            _protector = provider.CreateProtector("PasswordResetTokens");
        }

        public async Task<bool> GenerateResetTokenAsync(string email)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user is null) return false;

            var raw = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var token = _protector.Protect($"{raw}|{DateTime.UtcNow:O}");

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);
            await _db.SaveChangesAsync();

            var url = $"https://tonapp.com/resetpassword?token={Uri.EscapeDataString(token)}";
            await _email.SendAsync(user.Email, "Réinitialisation de votre mot de passe",
                $"<p>Cliquez <a href=\"{url}\">ici</a> pour changer votre mot de passe.</p>");

            return true;
        }

         public async Task<bool> ResetPasswordAsync(ResetPasswordDto model)
        {
            if (model.NewPassword != model.ConfirmPassword) return false;

            var user = await _db.Users.SingleOrDefaultAsync(u =>
                u.PasswordResetToken == model.Token &&
                u.PasswordResetTokenExpires > DateTime.UtcNow);

            if (user is null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
