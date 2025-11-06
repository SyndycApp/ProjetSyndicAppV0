namespace SyndicApp.Mobile.Models;

public sealed class ResetPasswordDto
{
    public string Token { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}
