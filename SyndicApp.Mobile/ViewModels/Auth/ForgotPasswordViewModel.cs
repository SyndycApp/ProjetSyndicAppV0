using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Auth;

public partial class ForgotPasswordViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IPasswordApi _api;

    [ObservableProperty] private string? email;
    [ObservableProperty] private bool hasError;
    [ObservableProperty] private string? errorMessage;

    public ForgotPasswordViewModel(IPasswordApi api)
    {
        _api = api;
        Title = "Mot de passe oublié";
    }

    // Génère SendResetCommand (comme dans ton XAML)
    [RelayCommand]
    private async Task SendResetAsync()
    {
        try
        {
            IsBusy = true;
            HasError = false;
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(Email))
            {
                HasError = true;
                ErrorMessage = "Veuillez saisir votre adresse email.";
                return;
            }

            // 👉 Envoi du CODE OTP
            await _api.ForgotCodeAsync(new ForgotPasswordDto { Email = Email! });

            await Shell.Current.DisplayAlert(
                "Code envoyé",
                $"Nous avons envoyé un code à {Mask(Email!)}.",
                "OK");

            // 👉 On passe à l’écran de saisie de code
            await Shell.Current.GoToAsync($"/verifycode?email={Uri.EscapeDataString(Email!)}");
        }
        catch
        {
            HasError = true;
            ErrorMessage = "Impossible d’envoyer le code. Vérifiez votre connexion.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Génère GoToLoginCommand (comme dans ton XAML)
    [RelayCommand]
    private Task GoToLoginAsync() => Shell.Current.GoToAsync("//login");

    private static string Mask(string email)
    {
        var at = email.IndexOf('@');
        if (at <= 1) return email;
        var local = email[..at];
        var domain = email[(at + 1)..];
        var head = local.Length <= 2 ? local[..1] : local[..2];
        return $"{head}{new string('*', Math.Max(1, local.Length - head.Length))}@{domain}";
    }
}
