using System.ComponentModel.DataAnnotations;

namespace SyndicApp.Application.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; init; } = string.Empty;
    }
}
