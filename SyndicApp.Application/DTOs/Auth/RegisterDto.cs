using System;
using System.ComponentModel.DataAnnotations;

namespace SyndicApp.Application.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = "";

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = "";

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? Adresse { get; set; }

        [Required(ErrorMessage = "La date de naissance est obligatoire.")]
        public DateTime? DateNaissance { get; set; }

        [Required]
        public string? Role { get; set; }
    }
}
