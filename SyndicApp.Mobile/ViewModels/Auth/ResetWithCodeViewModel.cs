using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Auth;

[QueryProperty(nameof(Email), "email")]
[QueryProperty(nameof(Code), "code")]
public partial class ResetWithCodeViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IPasswordApi _api;

    [ObservableProperty] string? email;
    [ObservableProperty] string? code;
    [ObservableProperty] string? newPassword;
    [ObservableProperty] string? confirmPassword;
    [ObservableProperty] bool hasError;
    [ObservableProperty] string? errorMessage;

    public ResetWithCodeViewModel(IPasswordApi api)
    {
        _api = api;
        Title = "Nouveau mot de passe";
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        try
        {
            HasError = false; ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword!.Length < 6)
            { HasError = true; ErrorMessage = "Mot de passe trop court (min 6)."; return; }

            if (NewPassword != ConfirmPassword)
            { HasError = true; ErrorMessage = "Les mots de passe ne correspondent pas."; return; }

            await _api.ResetWithCodeAsync(new ResetWithCodeDto
            {
                Email = Email!,
                Code = Code!,
                NewPassword = NewPassword!,
                ConfirmPassword = ConfirmPassword!
            });

            await Shell.Current.DisplayAlert("Succès", "Mot de passe réinitialisé.", "OK");
            await Shell.Current.GoToAsync("//login");
        }
        catch
        {
            HasError = true; ErrorMessage = "Erreur lors de la réinitialisation.";
        }
    }
}
