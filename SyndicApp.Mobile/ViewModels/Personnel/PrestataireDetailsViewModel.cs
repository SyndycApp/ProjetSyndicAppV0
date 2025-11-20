using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;  

namespace SyndicApp.Mobile.ViewModels.Personnel
{
    [QueryProperty(nameof(IdParam), "id")]
    public partial class PrestataireDetailsViewModel : ObservableObject
    {
        private readonly IPrestatairesApi _prestatairesApi;
        private readonly IAuthApi _authApi;

        public PrestataireDetailsViewModel(
            IPrestatairesApi prestatairesApi,
            IAuthApi authApi)
        {
            _prestatairesApi = prestatairesApi;
            _authApi = authApi;
        }

        [ObservableProperty] private string? idParam;
        [ObservableProperty] private Guid id;
        [ObservableProperty] private PrestataireDto? item;
        [ObservableProperty] private bool isBusy;

        public bool CanCreateAccount => Item != null /* && Item.UserId == null */;

        partial void OnItemChanged(PrestataireDto? value)
        {
            OnPropertyChanged(nameof(CanCreateAccount));
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (!Guid.TryParse(IdParam, out var guid))
            {
                await Shell.Current.DisplayAlert("Erreur", "Identifiant prestataire invalide.", "OK");
                return;
            }

            Id = guid;

            try
            {
                IsBusy = true;
                Item = await _prestatairesApi.GetByIdAsync(guid);
            }
            catch (ApiException apiEx)
            {
                await Shell.Current.DisplayAlert("API", apiEx.Content ?? apiEx.Message, "OK");
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
        public async Task DeleteAsync()
        {
            if (Item is null) return;

            var ok = await Shell.Current.DisplayAlert(
                "Supprimer",
                $"Supprimer le prestataire {Item.Nom} ?",
                "Supprimer", "Annuler");

            if (!ok) return;

            try
            {
                await _prestatairesApi.DeleteAsync(Item.Id);
                await Shell.Current.DisplayAlert("OK", "Prestataire supprimé.", "OK");
                await Shell.Current.GoToAsync("//prestataires");
            }
            catch (ApiException apiEx)
            {
                await Shell.Current.DisplayAlert("API", apiEx.Content ?? apiEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        // 🔥 Création du compte pour le prestataire
        [RelayCommand]
        public async Task CreateAccountAsync()
        {
            if (Item is null)
            {
                await Shell.Current.DisplayAlert("Info", "Prestataire non chargé.", "OK");
                return;
            }

            // 1) Email
            var email = await Shell.Current.DisplayPromptAsync(
                "Créer un compte",
                "Email du prestataire :",
                "Suivant", "Annuler",
                initialValue: Item.Email ?? string.Empty,
                keyboard: Keyboard.Email);

            if (string.IsNullOrWhiteSpace(email))
                return;

            // 2) Nom complet
            var fullName = await Shell.Current.DisplayPromptAsync(
                "Nom complet",
                "Nom complet du prestataire :",
                "Suivant", "Annuler",
                initialValue: Item.Nom);

            if (string.IsNullOrWhiteSpace(fullName))
                return;

            // 3) Mot de passe (⚠️ MAUI ne masque pas ici, c’est une simple prompt)
            var password = await Shell.Current.DisplayPromptAsync(
                "Mot de passe",
                "Mot de passe temporaire à communiquer au prestataire :",
                "Créer", "Annuler",
                maxLength: 50,
                keyboard: Keyboard.Text);

            if (string.IsNullOrWhiteSpace(password))
                return;

            var req = new CreatePrestataireAccountRequest
            {
                PrestataireId = Item.Id,
                Email = email.Trim(),
                FullName = fullName.Trim(),
                Password = password
            };

            try
            {
                IsBusy = true;

                // 🔥 méthode alignée avec ton AuthController: /api/Auth/register-from-prestataire-account
                var user = await _authApi.RegisterFromPrestataireAccountAsync(req);

                await Shell.Current.DisplayAlert(
                    "Compte créé",
                    $"Compte créé pour {user.Email} avec le rôle Prestataire.\n" +
                    $"Transmets l'email et le mot de passe au prestataire.",
                    "OK");

                // Recharger pour mettre à jour un éventuel statut HasAccount
                Item = await _prestatairesApi.GetByIdAsync(Item.Id);
            }
            catch (ApiException apiEx)
            {
                var msg = $"Code: {(int)apiEx.StatusCode} - {apiEx.StatusCode}\n\n" +
                             (apiEx.Content ?? apiEx.Message);
                await Shell.Current.DisplayAlert("API", msg, "OK");
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
        public async Task BackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
