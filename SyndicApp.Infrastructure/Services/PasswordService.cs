// Fichier : SyndicApp.Infrastructure/Services/PasswordService.cs
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
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public PasswordService(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<bool> GenerateResetTokenAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return false;

            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);

            await _context.SaveChangesAsync();

            var resetUrl = $"https://tonapp.com/resetpassword?token={user.PasswordResetToken}";

            await _emailSender.SendEmailAsync(user.Email, "Réinitialisation mot de passe",
                $"Cliquez sur ce lien pour réinitialiser votre mot de passe : {resetUrl}");

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto model)
        {
            if (string.IsNullOrEmpty(model.Token) ||
                string.IsNullOrEmpty(model.NewPassword) ||
                string.IsNullOrEmpty(model.ConfirmPassword) ||
                model.NewPassword != model.ConfirmPassword)
                return false;

            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.PasswordResetToken == model.Token &&
                u.PasswordResetTokenExpires > DateTime.UtcNow);

            if (user == null)
                return false;

            user.PasswordHash = HashPassword(model.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return true;
        }

        private string HashPassword(string password)
        {
            // Utilise BCrypt.Net-Next (à ajouter via NuGet)
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
