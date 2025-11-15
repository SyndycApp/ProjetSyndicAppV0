using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    public partial class ChargeEditViewModel : ObservableObject
    {
        private readonly IChargesApi _chargesApi;
        private readonly IResidencesApi _residencesApi;

        private Guid _id;

        [ObservableProperty]
        private string nom = string.Empty;

        [ObservableProperty]
        private decimal montant;

        [ObservableProperty]
        private DateTime dateCharge = DateTime.Today;

        [ObservableProperty]
        private ObservableCollection<string> residences = new();

        [ObservableProperty]
        private string? selectedResidence;

        [ObservableProperty]
        private bool isBusy;

        public ChargeEditViewModel(IChargesApi chargesApi, IResidencesApi residencesApi)
        {
            _chargesApi = chargesApi;
            _residencesApi = residencesApi;
        }

        public async Task InitializeAsync(Guid id)
        {
            if (IsBusy) return;

            _id = id;

            try
            {
                IsBusy = true;
                Residences.Clear();

                var listResidences = await _residencesApi.GetAllAsync();
                foreach (var r in listResidences)
                    Residences.Add(r.Nom);

                var charge = await _chargesApi.GetByIdAsync(id);

                Nom = charge.Nom;
                Montant = charge.Montant;
                DateCharge = charge.DateCharge;

                if (!string.IsNullOrWhiteSpace(charge.ResidenceNom) &&
                    Residences.Contains(charge.ResidenceNom))
                {
                    SelectedResidence = charge.ResidenceNom;
                }
                else
                {
                    // fallback : trouver via LookupIdAsync -> pas idéal, mais safe
                    // ici on ne fait rien si on n'a pas le nom
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
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
    }
}
