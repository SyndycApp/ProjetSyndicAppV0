using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    [QueryProperty(nameof(PaiementId), "id")]
    public partial class PaiementDetailsViewModel : ObservableObject
    {
        private readonly IPaiementsApi _paiementsApi;
        private readonly IAppelsApi _appelsApi;
        private readonly IResidencesApi _residencesApi;
        private readonly IAuthApi _authApi;

        public PaiementDetailsViewModel(
            IPaiementsApi paiementsApi,
            IAppelsApi appelsApi,
            IResidencesApi residencesApi,
            IAuthApi authApi)
        {
            _paiementsApi = paiementsApi;
            _appelsApi = appelsApi;
            _residencesApi = residencesApi;
            _authApi = authApi;
        }

        // ID venant de la navigation
        [ObservableProperty] private string? paiementId;

        // Paiement
        [ObservableProperty] private decimal montant;
        [ObservableProperty] private DateTime datePaiement;
        [ObservableProperty] private string? userFullName;

        // Appel de fonds
        [ObservableProperty] private string? appelDescription;
        [ObservableProperty] private decimal montantTotal;
        [ObservableProperty] private decimal montantPaye;
        [ObservableProperty] private decimal montantReste;
        [ObservableProperty] private DateTime dateEmission;
        [ObservableProperty] private int nbPaiements;
        [ObservableProperty] private Guid residenceId;
        [ObservableProperty] private string? residenceNom;

        // Résidence
        [ObservableProperty] private string? residenceAdresse;
        [ObservableProperty] private string? residenceVille;
        [ObservableProperty] private string? residenceCodePostal;

        // Load details
        [RelayCommand]
        public async Task LoadAsync()
        {
            if (string.IsNullOrWhiteSpace(PaiementId)) return;
            if (!Guid.TryParse(PaiementId, out var id)) return;

            // ====== Paiement ======
            var paiement = await _paiementsApi.GetByIdAsync(id);
            Montant = paiement.Montant;
            DatePaiement = paiement.DatePaiement;
            UserFullName = paiement.NomCompletUser;

            // ====== Appel de Fonds ======
            var appel = await _appelsApi.GetByIdAsync(paiement.AppelDeFondsId.ToString());
            AppelDescription = appel.Description;
            MontantTotal = appel.MontantTotal;
            MontantPaye = appel.MontantPaye;
            MontantReste = appel.MontantReste;
            DateEmission = appel.DateEmission;
            ResidenceId = appel.ResidenceId;
            NbPaiements = appel.NbPaiements;
            ResidenceNom = appel.ResidenceNom;

            // ====== Résidence ======
            var residence = await _residencesApi.GetByIdAsync(ResidenceId.ToString());
            ResidenceAdresse = residence.Adresse;
            ResidenceVille = residence.Ville;
            ResidenceCodePostal = residence.CodePostal;
        }

        [RelayCommand]
        public Task GoBack() => Shell.Current.GoToAsync("..");
    }
}
