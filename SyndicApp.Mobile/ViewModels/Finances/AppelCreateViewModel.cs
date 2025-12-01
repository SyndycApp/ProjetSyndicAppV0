using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    public partial class AppelCreateViewModel : ObservableObject
    {
        private readonly IAppelsApi _api;
        private readonly IResidencesApi _residencesApi;

        [ObservableProperty] private string? description;
        [ObservableProperty] private DateTime dateEmission = DateTime.Today;
        [ObservableProperty] private string residenceId = string.Empty;
        [ObservableProperty] private string montantTotalText = string.Empty;
        [ObservableProperty] private bool isBusy;

        [ObservableProperty] private List<ResidenceDto> residences = new();
        [ObservableProperty] private ResidenceDto? selectedResidence;

        [ObservableProperty] private bool isSyndic;

        public AppelCreateViewModel(IAppelsApi api, IResidencesApi residencesApi)
        {
            _api = api;
            _residencesApi = residencesApi;

            var role = Preferences.Get("user_role", string.Empty)?.ToLowerInvariant() ?? string.Empty;
            IsSyndic = role.Contains("syndic");

            _ = LoadResidencesAsync();
        }

        [RelayCommand]
        public async Task LoadResidencesAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Residences = await _residencesApi.GetAllAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    "Impossible de charger les résidences : " + ex.Message,
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CreateAsync()
        {
            if (IsBusy) return;

            if (!IsSyndic)
            {
                await Shell.Current.DisplayAlert("Accès restreint", "Seul le syndic peut créer un appel de fonds.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                await Shell.Current.DisplayAlert("Champs requis", "La description est obligatoire.", "OK");
                return;
            }

            if (SelectedResidence is null)
            {
                await Shell.Current.DisplayAlert("Champs requis", "La résidence est obligatoire.", "OK");
                return;
            }

            if (!TryParseDecimal(MontantTotalText, out var montant))
            {
                await Shell.Current.DisplayAlert(
                    "Montant invalide",
                    "Saisis un montant valide (ex: 1234,56).",
                    "OK");
                return;
            }

            try
            {
                IsBusy = true;

                var nom = SelectedResidence.Nom ?? string.Empty;
                var residenceGuid = await _residencesApi.LookupIdAsync(nom);
                ResidenceId = residenceGuid.ToString();

                var payload = new AppelDeFondsDto
                {
                    Description = Description,
                    DateEmission = DateEmission,
                    ResidenceId = ResidenceId,
                    MontantTotal = montant
                };

                var created = await _api.CreateAsync(payload);

                if (!string.IsNullOrWhiteSpace(created?.Id))
                    await Shell.Current.GoToAsync($"appel-details?id={created.Id}");
                else
                    await Shell.Current.GoToAsync("//appels");
            }
            catch (ApiException ex)
            {
                var message = string.IsNullOrWhiteSpace(ex.Content) ? ex.Message : ex.Content;
                await Shell.Current.DisplayAlert("Erreur API", message, "OK");
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

        private static bool TryParseDecimal(string? input, out decimal value)
        {
            input ??= string.Empty;
            input = input.Trim().Replace(" ", string.Empty);

            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out value))
                return true;
            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                return true;

            var swapped = input.Replace(',', '.');
            return decimal.TryParse(swapped, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }
    }
}
