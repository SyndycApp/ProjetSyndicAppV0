using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;      // IAuthApi + LoginRequest déjà utilisés chez toi
using Refit;
using System.Collections.ObjectModel;
using SyndicApp.Mobile.Api.Models;

namespace SyndicApp.Mobile.ViewModels.Auth;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IAuthApi _auth;

    // === champs du modèle API ===
    [ObservableProperty] private string? fullName;
    [ObservableProperty] private string? adresse;
    [ObservableProperty] private string? email;
    [ObservableProperty] private string? password;
    [ObservableProperty] private string? confirmPassword;
    [ObservableProperty] private DateTime dateNaissance = DateTime.Today.AddYears(-18);
    [ObservableProperty] private string? role = "Copropriétaire";

    // === UI helpers ===
    public ObservableCollection<string> Roles { get; } =
        new(new[] { "Syndic", "Copropriétaire", "Gardien", "Locataire" });

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    public bool CanRegister =>
        !IsBusy &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Password) &&
        !string.IsNullOrWhiteSpace(ConfirmPassword) &&
        Password == ConfirmPassword &&
        !string.IsNullOrWhiteSpace(FullName) &&
        !string.IsNullOrWhiteSpace(Role);

    public RegisterViewModel(IAuthApi auth)
    {
        _auth = auth;
        // pour rafraîchir CanRegister quand une propriété change
        PropertyChanged += (_, __) => OnPropertyChanged(nameof(CanRegister));
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (!CanRegister)
        {
            Error = "Veuillez remplir tous les champs et vérifier les mots de passe.";
            OnPropertyChanged(nameof(HasError));
            return;
        }

        try
        {
            IsBusy = true; Error = null;

            // IMPORTANT : utilise la classe du namespace API
            // using SyndicApp.Mobile.Api;

            var payload = new RegisterDto
            {
                Email = Email!.Trim(),
                Password = Password!,
                ConfirmPassword = ConfirmPassword!,
                FullName = FullName!.Trim(),
                Adresse = string.IsNullOrWhiteSpace(Adresse) ? null : Adresse!.Trim(),
                DateNaissance = DateNaissance,
                Role = Role!
            };

            await _auth.Register(payload);

            await Shell.Current.DisplayAlert("Succès", "Compte créé. Connectez-vous.", "OK");
            await Shell.Current.GoToAsync("//login");
        }

        catch (ApiException ex)
        {
            Error = ex.Content ?? ex.Message;
            OnPropertyChanged(nameof(HasError));
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            OnPropertyChanged(nameof(HasError));
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private Task GoToLogin() => Shell.Current.GoToAsync("//login");
}


