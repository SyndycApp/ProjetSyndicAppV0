namespace SyndicApp.Mobile.Models;

public sealed class ResetWithCodeDto
{
    public string Email { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}
