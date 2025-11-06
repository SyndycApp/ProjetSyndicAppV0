namespace SyndicApp.Application.DTOs.Auth;
public sealed class VerifyResetCodeDto {
    public string Email { get; set; } = default!; 
    public string Code { get; set; } = default!; 
}
