using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api;

public interface IPasswordApi
{
    // Envoi du mail de réinitialisation
    [Post("/api/Password/forgot")]
    Task<ApiOkDto> ForgotPasswordAsync([Body] ForgotPasswordDto dto);

    // Réinitialisation via token
    [Post("/api/Password/reset")]
    Task<ApiOkDto> ResetPasswordAsync([Body] ResetPasswordDto dto);
}