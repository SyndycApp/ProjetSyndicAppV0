using System;

namespace SyndicApp.Application.DTOs.Auth
{
    public class PrestataireRegisterRequest
    {
        public Guid PrestataireId { get; set; }

        // Optionnel : si vide, on prendra Email du prestataire
        public string? EmailOverride { get; set; }

        // Optionnel : si vide, on génèrera un mot de passe aléatoire
        public string? Password { get; set; }
    }

    public class PrestataireRegisterResponse
    {
        public Guid UserId { get; set; }
        public Guid PrestataireId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string TempPassword { get; set; } = string.Empty; // à noter et communiquer
    }
}
