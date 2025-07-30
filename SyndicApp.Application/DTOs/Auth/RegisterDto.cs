using System;

namespace SyndicApp.Application.DTOs.Auth
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string FullName { get; set; } = string.Empty;
        public string? Adresse { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string Role { get; set; } = "Copropriétaire"; // Par défaut
    }
}
