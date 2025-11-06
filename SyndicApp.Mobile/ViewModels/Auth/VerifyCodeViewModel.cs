using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Auth;

[QueryProperty(nameof(Email), "email")]
public partial class VerifyCodeViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IPasswordApi _api;

    [ObservableProperty] string? email;
    [ObservableProperty] string? code;
    [ObservableProperty] bool hasError;
    [ObservableProperty] string? errorMessage;

    public VerifyCodeViewModel(IPasswordApi api)
    {
        _api = api;
        Title = "Code de vérification";
    }

    [RelayCommand]
    private async Task VerifyAsync()
    {
        try
        {
            HasError = false; ErrorMessage = null;
            if (string.IsNullOrWhiteSpace(Code) || Code!.Length != 6)
            { HasError = true; ErrorMessage = "Code à 6 chiffres requis."; return; }

            await _api.VerifyCodeAsync(new VerifyResetCodeDto { Email = Email!, Code = Code! });

            await Shell.Current.GoToAsync($"/resetpwd?email={Uri.EscapeDataString(Email!)}&code={Uri.EscapeDataString(Code!)}");
        }
        catch
        {
            HasError = true; ErrorMessage = "Code invalide ou expiré.";
        }
    }

    [RelayCommand]
    private async Task ResendAsync()
    {
        try
        {
            await _api.ForgotCodeAsync(new ForgotPasswordDto { Email = Email! });
            await Shell.Current.DisplayAlert("Code renvoyé", "Vérifie ta boîte mail.", "OK");
        }
        catch { /* silencieux */ }
    }
}
