namespace SyndicApp.Mobile.Models;

public sealed class LoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public sealed class RegisterDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string? Adresse { get; set; }
    public DateTime DateNaissance { get; set; }
    public string Role { get; set; } = "Copropriétaire"; // par défaut
}

public sealed class ResetPasswordDto
{
    public string Token { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}

public sealed class AuthResponseDto
{
    public string Token { get; set; } = default!;
    public UserDto? User { get; set; }
}

public sealed class ApiOkDto
{
    public bool Ok { get; set; } = true;
}
