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
    private readonly IAuthApi _authApi;
    private readonly IAccountApi _accountApi;
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

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                HasError = true;
                ErrorMessage = "Saisis l’email et le mot de passe.";
                return;
            }

            var resp = await _authApi.LoginAsync(new LoginDto
            {
                Email = Email,
                Password = Password
            });

            _tokenStore.SaveToken(resp.Token ?? string.Empty);

            var token = _tokenStore.GetToken();
            if (string.IsNullOrWhiteSpace(token))
            {
                HasError = true;
                ErrorMessage = "Token non reçu. Vérifie l’API.";
                return;
            }

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

            App.UserId = me.Id.ToString();

            var role = me.Roles?.FirstOrDefault()?.Trim();
            if (!string.IsNullOrEmpty(role))
            {
                _tokenStore.SaveRole(role);
            }

            // ========= REDIRECTION SELON LE RÔLE =========
            var route = GetHomeRouteForRole(role);
            await Shell.Current.GoToAsync(route);
        }
        catch (ApiException ex)
        {
            if ((int)ex.StatusCode is >= 400 and < 500)
            {
                HasError = true;
                ErrorMessage = "Email ou mot de passe incorrect.";
            }
            else
            {
                await Shell.Current.DisplayAlert(
                    $"Erreur {(int)ex.StatusCode}",
                    ex.Content ?? "Erreur serveur",
                    "OK");
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

    private static string GetHomeRouteForRole(string? role)
    {
        if (string.IsNullOrWhiteSpace(role))
            return "//syndic-dashboard";

        var r = role.Trim().ToLowerInvariant();

        if (r.Contains("syndic"))
            return "//syndic-dashboard";

        if (r.Contains("copro"))
            return "//affectation-user-dashboard";

        if (r.Contains("gardien") || r.Contains("maintenance"))
            return "//affectation-maintenance-dashboard";

        if (r.Contains("locataire"))
            return "//affectation-user-dashboard";

        if (r.Contains("prestataire"))
            return "//prestataires";

        // fallback
        return "//syndic-dashboard";
    }

    [RelayCommand]
    public Task GoToRegisterAsync() => Shell.Current.GoToAsync("//register");

    [RelayCommand]
    public Task GoToForgotPasswordAsync() => Shell.Current.GoToAsync("//forgot");
}
