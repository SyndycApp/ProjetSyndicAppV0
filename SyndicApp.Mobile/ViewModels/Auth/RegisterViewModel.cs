using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.ViewModels.Auth;

public partial class RegisterViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IAuthApi _authApi;
    private readonly IAccountApi _accountApi; // /me via AuthHeaderHandler (Bearer auto)
    private readonly TokenStore _tokenStore;

    [ObservableProperty] private string? email;
    [ObservableProperty] private string? password;
    [ObservableProperty] private string? confirmPassword;
    [ObservableProperty] private string? fullName;
    [ObservableProperty] private string? adresse;
    [ObservableProperty] private DateTime dateNaissance = DateTime.Today.AddYears(-20);
    [ObservableProperty] private string? selectedRole = "Copropriétaire";

    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private bool hasError;

    public IReadOnlyList<string> Roles { get; } = new[] { "Syndic", "Copropriétaire", "Gardien", "Locataire" };

    public RegisterViewModel(IAuthApi authApi, IAccountApi accountApi, TokenStore tokenStore)
    {
        _authApi = authApi;
        _accountApi = accountApi;
        _tokenStore = tokenStore;
        Title = "Créer un compte";
    }

    [RelayCommand]
    public async Task RegisterAsync()
    {
        IsBusy = true;
        HasError = false;
        ErrorMessage = null;

        try
        {
            // validations basiques
            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword) ||
                string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(SelectedRole))
            {
                HasError = true;
                ErrorMessage = "Tous les champs marqués * sont obligatoires.";
                return;
            }
            if (Password!.Length < 6)
            {
                HasError = true;
                ErrorMessage = "Mot de passe trop court (min. 6).";
                return;
            }
            if (!string.Equals(Password, ConfirmPassword))
            {
                HasError = true;
                ErrorMessage = "Les mots de passe ne correspondent pas.";
                return;
            }

            var dto = new RegisterDto
            {
                Email = Email!,
                Password = Password!,
                ConfirmPassword = ConfirmPassword!,
                FullName = FullName!,
                Adresse = Adresse,
                DateNaissance = DateNaissance,
                Role = SelectedRole!
            };

            // Appel register (pas d’auto-login derrière)
            await _authApi.RegisterAsync(dto);

            // Par sécurité, si un token traînait, on le nettoie
            _tokenStore.ClearToken();

            await Shell.Current.DisplayAlert("Compte créé", "Ton compte est créé. Tu peux te connecter maintenant.", "OK");
            await Shell.Current.GoToAsync("//login");   // ⇐ navigation vers Login

            // Optionnel : reset des champs du formulaire
            Email = Password = ConfirmPassword = FullName = Adresse = null;
            DateNaissance = DateTime.Today.AddYears(-20);
            SelectedRole = "Copropriétaire";
        }
        catch (ApiException ex) when ((int)ex.StatusCode >= 400 && (int)ex.StatusCode < 500)
        {
            try
            {
                var err = System.Text.Json.JsonSerializer.Deserialize<ApiError>(ex.Content,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ErrorMessage = err?.Details?.FirstOrDefault()
                               ?? err?.Message
                               ?? ex.Content
                               ?? "Inscription invalide. Vérifie les informations saisies.";
            }
            catch
            {
                ErrorMessage = ex.Content ?? "Inscription invalide. Vérifie les informations saisies.";
            }
            HasError = true;
        }
        catch (HttpRequestException)
        {
            HasError = true;
            ErrorMessage = "Impossible de contacter l'API. Vérifie la connexion.";
        }
        catch (TaskCanceledException)
        {
            HasError = true;
            ErrorMessage = "Temps d’attente dépassé. API injoignable.";
        }
        finally
        {
            IsBusy = false;
        }
    }


    [RelayCommand]
    public Task GoToLoginAsync() => Shell.Current.GoToAsync("//login");
}
