using System.ComponentModel.DataAnnotations;
namespace SyndicApp.Application.DTOs.Auth
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; init; } = string.Empty;

        [Required, MinLength(8)]
        public string NewPassword { get; init; } = string.Empty;

        [Required, Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; init; } = string.Empty;
    }
}