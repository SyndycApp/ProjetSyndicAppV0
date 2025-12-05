using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    [QueryProperty(nameof(Id), "id")]
    public partial class AppelDetailsViewModel : ObservableObject
    {
        private readonly IAppelsApi _appelsApi;
        private readonly IPaiementsApi _paiementsApi;

        [ObservableProperty]
        private string id = string.Empty;

        [ObservableProperty]
        private AppelDeFondsDto? appel;

        // Liste observable MAUI (mise à jour automatique)
        [ObservableProperty]
        private ObservableCollection<PaiementDto> paiements = new();

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isSyndic;

        public AppelDetailsViewModel(IAppelsApi appelsApi, IPaiementsApi paiementsApi)
        {
            _appelsApi = appelsApi;
            _paiementsApi = paiementsApi;

            IsSyndic = Preferences.Get("user_role", "").ToLowerInvariant().Contains("syndic");
        }

        // chargement automatique dès que l'ID change
        partial void OnIdChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _ = LoadAsync();
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(Id))
                return;

            try
            {
                IsBusy = true;

                // Charger l’appel
                var appelData = await _appelsApi.GetByIdAsync(Id);
                Appel = appelData;

                // 🔥 APPEL CORRIGÉ — EN STRING !!
                var liste = await _paiementsApi.GetByAppelIdAsync(Id);

                // Debug
                await Shell.Current.DisplayAlert("DEBUG", $"Paiements = {liste.Count}", "OK");

                Paiements = new ObservableCollection<PaiementDto>(liste);

                Appel.NbPaiements = liste.Count;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur API", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }



        // --------------------------------------
        // ACTIONS SYNDIC
        // --------------------------------------
        [RelayCommand]
        public async Task EditAsync()
        {
            if (!IsSyndic) return;
            await Shell.Current.GoToAsync($"appel-edit?id={Id}");
        }

        [RelayCommand]
        public async Task CloturerAsync()
        {
            if (!IsSyndic) return;

            await _appelsApi.CloturerAsync(Id);
            await LoadAsync();
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (!IsSyndic) return;

            try
            {
                await _appelsApi.DeleteAsync(Id);
                await Shell.Current.GoToAsync("//appels");
            }
            catch (ApiException apiEx)
            {
                var error = apiEx.Content; // le JSON renvoyé par l’API

                if (!string.IsNullOrWhiteSpace(error))
                    await Shell.Current.DisplayAlert("Impossible de supprimer", error, "OK");
                else
                    await Shell.Current.DisplayAlert("Erreur", apiEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }
    }
}
