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
}