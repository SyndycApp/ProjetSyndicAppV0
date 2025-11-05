using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.ViewModels.Auth;

public partial class LoginViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IAuthApi _authApi;       // public (login/register)
    private readonly IAccountApi _accountApi; // protégé (Bearer via AuthHeaderHandler)
    private readonly TokenStore _tokenStore;

    [ObservableProperty] private string? email;
    [ObservableProperty] private string? password;

    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private bool hasError;

    public LoginViewModel(IAuthApi authApi, IAccountApi accountApi, TokenStore tokenStore)
    {
        _authApi = authApi;
        _accountApi = accountApi;
        _tokenStore = tokenStore;
        Title = "Connexion";
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task LoginAsync()
    {
        try
        {
            IsBusy = true;
            HasError = false;
            ErrorMessage = null;

            // 0) Validation basique
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                HasError = true;
                ErrorMessage = "Saisis l’email et le mot de passe.";
                return;
            }

            // 1) Login (public) → récup token
            var resp = await _authApi.LoginAsync(new LoginDto { Email = Email, Password = Password });
            _tokenStore.SaveToken(resp.Token);

            var token = _tokenStore.GetToken();
            if (string.IsNullOrWhiteSpace(token))
            {
                HasError = true;
                ErrorMessage = "Token non reçu. Vérifie l’API.";
                return;
            }

            // 2) /api/Auth/me via handler (pas d’argument)
            UserDto me;
            try
            {
                me = await _accountApi.MeAsync();
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                HasError = true;
                ErrorMessage = "Email ou mot de passe incorrect, ou token non transmis.";
                return;
            }

            // 3) Persister le rôle (1er élément de la liste) et router
            var role = me.Roles?.FirstOrDefault()?.Trim(); // ex: "Syndic"
            if (!string.IsNullOrEmpty(role))
                _tokenStore.SaveRole(role);

            // Navigation (ajuste si tu as plusieurs dashboards)
            await Shell.Current.GoToAsync("//syndic-dashboard");
        }
        catch (ApiException ex)
        {
            // 4xx → message bandeau ; 5xx / autres → alerte
            if ((int)ex.StatusCode >= 400 && (int)ex.StatusCode < 500)
            {
                HasError = true;
                ErrorMessage = "Email ou mot de passe incorrect.";
            }
            else
            {
                await Shell.Current.DisplayAlert($"Erreur {(int)ex.StatusCode}", ex.Content ?? "Erreur serveur", "OK");
            }
        }
        catch (HttpRequestException httpEx)
        {
            HasError = true;
            ErrorMessage = "Impossible de contacter l'API.";
            await Shell.Current.DisplayAlert(
                "Réseau",
                $"Vérifie la BaseUrl (côté app) et que l'API écoute sur 0.0.0.0:5041.\n\nDétail : {httpEx.Message}",
                "OK");
        }
        catch (TaskCanceledException)
        {
            HasError = true;
            ErrorMessage = "Temps d’attente dépassé. API injoignable depuis l’appareil.";
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public Task GoToRegisterAsync() => Shell.Current.GoToAsync("//register");

    [RelayCommand]
    public Task GoToForgotPasswordAsync() => Shell.Current.GoToAsync("//forgot");
}
