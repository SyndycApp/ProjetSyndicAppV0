using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api;

public interface IPasswordApi
{
    // Envoi du mail de réinitialisation
    [Post("/api/Password/forgot")]
    Task ForgotPasswordAsync([Body] ForgotPasswordDto dto);

    [Post("/api/Password/reset")]
    Task ResetPasswordAsync([Body] ResetPasswordDto dto);

    [Post("/api/Password/forgot-code")]
    Task ForgotCodeAsync([Body] ForgotPasswordDto dto);

    [Post("/api/Password/verify-code")]
    Task VerifyCodeAsync([Body] VerifyResetCodeDto dto);

    [Post("/api/Password/reset-with-code")]
    Task ResetWithCodeAsync([Body] ResetWithCodeDto dto);

    [Post("/api/Auth/logout")]
    Task<ApiOkDto> LogoutAsync();
}