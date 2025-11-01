using System.Text.Json.Serialization;

namespace SyndicApp.Mobile.Api.Models;

public class RegisterDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = default!;

    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; } = default!;

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = default!;

    [JsonPropertyName("adresse")]
    public string? Adresse { get; set; }

    [JsonPropertyName("dateNaissance")]
    public DateTime DateNaissance { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; } = default!;
}
