using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Batiments
{
    public partial class BatimentCreateViewModel : ObservableObject
    {
        private readonly IBatimentsApi _api;

        [ObservableProperty] string nom = string.Empty;
        // on tape le GUID directement (comme demandé)
        [ObservableProperty] string residenceId = string.Empty;

        public BatimentCreateViewModel(IBatimentsApi api) => _api = api;

        [RelayCommand]
        private async Task CreateAsync()
        {
            if (string.IsNullOrWhiteSpace(Nom))
            {
                await Shell.Current.DisplayAlert("Validation", "Le nom est obligatoire.", "OK");
                return;
            }

            if (!Guid.TryParse(ResidenceId, out var residenceGuid))
            {
                await Shell.Current.DisplayAlert("Validation", "ResidenceId doit être un GUID valide.", "OK");
                return;
            }

            try
            {
                var dto = new BatimentCreateDto
                {
                    Nom = Nom.Trim(),
                    ResidenceId = residenceGuid
                };

                await _api.CreateAsync(dto);

                // 🔔 prévenir la liste sans changer ton flow
                WeakReferenceMessenger.Default.Send(new BatimentChangedMessage(true));

                await Shell.Current.DisplayAlert("Succès", "Bâtiment créé.", "OK");
                await Shell.Current.GoToAsync("//batiments");
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Erreur API",
                    string.IsNullOrWhiteSpace(ex.Content) ? ex.Message : ex.Content, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private Task CancelAsync() => Shell.Current.GoToAsync("//batiments");
    }
}
