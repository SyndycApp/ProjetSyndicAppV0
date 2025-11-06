using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Auth;

[QueryProperty(nameof(Token), "token")]
[QueryProperty(nameof(Email), "email")]
public partial class ResetPasswordViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IPasswordApi _passwordApi;

    [ObservableProperty] private string? token;
    [ObservableProperty] private string? email;
    [ObservableProperty] private string? newPassword;
    [ObservableProperty] private string? confirmPassword;
    [ObservableProperty] private bool hasError;
    [ObservableProperty] private string? errorMessage;

    public ResetPasswordViewModel(IPasswordApi passwordApi)
    {
        _passwordApi = passwordApi;
        Title = "Nouveau mot de passe";
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        try
        {
            HasError = false; ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(Token))
            { HasError = true; ErrorMessage = "Lien invalide ou expiré."; return; }

            if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword != ConfirmPassword)
            { HasError = true; ErrorMessage = "Les mots de passe ne correspondent pas."; return; }

            await _passwordApi.ResetPasswordAsync(new ResetPasswordDto
            {
                Token = Token!,
                NewPassword = NewPassword!,
                ConfirmPassword = ConfirmPassword!
            });

            await Shell.Current.DisplayAlert("Succès", "Votre mot de passe a été réinitialisé.", "OK");
            await Shell.Current.GoToAsync("//login");
        }
        catch (TaskCanceledException)
        {
            HasError = true; ErrorMessage = "Délai dépassé. Vérifiez votre connexion.";
        }
        catch (Exception)
        {
            HasError = true; ErrorMessage = "Impossible de réinitialiser. Le lien est peut-être expiré.";
        }
    }
}
