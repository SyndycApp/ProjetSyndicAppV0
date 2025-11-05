using Android.Telephony;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Auth;

public partial class ForgotPasswordViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IPasswordApi _passwordApi;

    [ObservableProperty] string? email;
    [ObservableProperty] bool hasError;
    [ObservableProperty] string? errorMessage;
    [ObservableProperty] bool isSent;

    public ForgotPasswordViewModel(IPasswordApi passwordApi)
    {
        _passwordApi = passwordApi;
        Title = "Mot de passe oublié";
    }

    [RelayCommand]
    public async Task SendResetAsync()
    {
        try
        {
            IsBusy = true;
            HasError = false;
            ErrorMessage = null;
            IsSent = false;

            if (string.IsNullOrWhiteSpace(Email))
            {
                HasError = true;
                ErrorMessage = "Veuillez saisir votre adresse email.";
                return;
            }

            await _passwordApi.ForgotPasswordAsync(new ForgotPasswordDto { Email = Email });
            IsSent = true;

            await Shell.Current.DisplayAlert(
                "Email envoyé",
                "Un lien de réinitialisation a été envoyé à ton adresse email.",
                "OK");

            await Shell.Current.GoToAsync("//login");
        }
        catch (ApiException ex)
        {
            HasError = true;
            ErrorMessage = ex.StatusCode switch
            {
                System.Net.HttpStatusCode.NotFound => "Aucun compte trouvé avec cet email.",
                _ => "Erreur lors de la demande de réinitialisation."
            };
        }
        catch (HttpRequestException)
        {
            HasError = true;
            ErrorMessage = "Impossible de contacter l’API.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public Task GoToLoginAsync() => Shell.Current.GoToAsync("//login");
}
