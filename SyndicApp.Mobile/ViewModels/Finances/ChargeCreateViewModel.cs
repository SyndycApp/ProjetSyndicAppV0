using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    public partial class ChargeCreateViewModel : ObservableObject
    {
        private readonly IChargesApi _chargesApi;
        private readonly IResidencesApi _residencesApi;

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

        public ChargeCreateViewModel(IChargesApi chargesApi, IResidencesApi residencesApi)
        {
            _chargesApi = chargesApi;
            _residencesApi = residencesApi;
        }

        public async Task InitializeAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Residences.Clear();

                var list = await _residencesApi.GetAllAsync();
                foreach (var r in list)
                {
                    var nom = r.Nom;
                    if (!string.IsNullOrWhiteSpace(nom))
                        Residences.Add(nom);
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

            var request = new ChargeCreateRequest
            {
                Nom = Nom.Trim(),
                Montant = Montant,
                DateCharge = DateCharge,
                ResidenceId = residenceId
            };

            await _chargesApi.CreateAsync(request);

            await Shell.Current.DisplayAlert(
                "Succès",
                "Charge créée avec succès.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
    }
}
