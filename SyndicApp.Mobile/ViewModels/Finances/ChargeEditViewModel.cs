using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    public partial class ChargeEditViewModel : ObservableObject
    {
        private readonly IChargesApi _chargesApi;
        private readonly IResidencesApi _residencesApi;

        private Guid _id;

        [ObservableProperty] private string nom = string.Empty;
        [ObservableProperty] private decimal montant;
        [ObservableProperty] private DateTime dateCharge = DateTime.Today;
        [ObservableProperty] private ObservableCollection<string> residences = new();
        [ObservableProperty] private string? selectedResidence;
        [ObservableProperty] private bool isBusy;

        // 🔐 rôle
        [ObservableProperty]
        private bool isSyndic;

        public bool CanManageCharges => IsSyndic;

        public ChargeEditViewModel(IChargesApi chargesApi, IResidencesApi residencesApi)
        {
            _chargesApi = chargesApi;
            _residencesApi = residencesApi;

            var role = Preferences.Get("user_role", string.Empty)
                                  ?.ToLowerInvariant()
                                  ?? string.Empty;

            IsSyndic = role.Contains("syndic");
        }

        public async Task InitializeAsync(Guid id)
        {
            if (IsBusy) return;

            _id = id;

            try
            {
                IsBusy = true;
                Residences.Clear();

                var listResidences = await _residencesApi.GetAllAsync() ?? new();
                foreach (var r in listResidences)
                {
                    if (!string.IsNullOrWhiteSpace(r.Nom))
                        Residences.Add(r.Nom);
                }

                var charge = await _chargesApi.GetByIdAsync(id);

                Nom = charge.Nom;
                Montant = charge.Montant;
                DateCharge = charge.DateCharge;

                if (!string.IsNullOrWhiteSpace(charge.ResidenceNom) &&
                    Residences.Contains(charge.ResidenceNom))
                {
                    SelectedResidence = charge.ResidenceNom;
                }
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur API",
                    ex.Content ?? ex.Message,
                    "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    ex.Message,
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (IsBusy) return;

            if (!CanManageCharges)
            {
                await Shell.Current.DisplayAlert(
                    "Accès restreint",
                    "Vous n'avez pas les droits pour modifier cette charge.",
                    "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Nom) ||
                Montant <= 0 ||
                string.IsNullOrWhiteSpace(SelectedResidence))
            {
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    "Merci de remplir tous les champs avec des valeurs valides.",
                    "OK");
                return;
            }

            try
            {
                IsBusy = true;

                var residenceId = await _residencesApi.LookupIdAsync(SelectedResidence!);

                var request = new ChargeUpdateRequest
                {
                    Nom = Nom.Trim(),
                    Montant = Montant,
                    DateCharge = DateCharge,
                    ResidenceId = residenceId
                };

                await _chargesApi.UpdateAsync(_id, request);

                await Shell.Current.DisplayAlert(
                    "Succès",
                    "Charge modifiée avec succès.",
                    "OK");

                await Shell.Current.GoToAsync("..");
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur API",
                    ex.Content ?? ex.Message,
                    "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                    "Erreur",
                    ex.Message,
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
