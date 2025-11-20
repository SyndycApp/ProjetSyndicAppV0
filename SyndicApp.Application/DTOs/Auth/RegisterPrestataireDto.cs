using System;

namespace SyndicApp.Application.DTOs.Auth
{
    public class RegisterPrestataireDto
    {
        public Guid PrestataireId { get; set; }     // Id du prestataire (dans table Prestataires)
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = "Prestataire@123"; // mot de passe auto si tu veux
    }
}
