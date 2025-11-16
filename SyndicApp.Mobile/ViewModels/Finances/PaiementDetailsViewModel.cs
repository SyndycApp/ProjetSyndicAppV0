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

        // ⚠️ maintenant STRING, pas Guid
        [ObservableProperty] private string? paiementId;

        // Paiement
        [ObservableProperty] private decimal montant;
        [ObservableProperty] private DateTime datePaiement;

        // Utilisateur
        [ObservableProperty] private string? userFullName;
        [ObservableProperty] private string? userRole;
        [ObservableProperty] private string? userAdresse;

        // Appel de fonds
        [ObservableProperty] private string? appelDescription;
        [ObservableProperty] private decimal montantTotal;
        [ObservableProperty] private decimal montantReste;
        [ObservableProperty] private int nbPaiements;
        [ObservableProperty] private DateTime dateEmission;

        // Résidence
        [ObservableProperty] private string? residenceNom;
        [ObservableProperty] private string? residenceAdresseComplete;

        [RelayCommand]
        public async Task LoadAsync()
        {
            // On parse l'id reçu dans la route
            if (string.IsNullOrWhiteSpace(PaiementId) ||
                !Guid.TryParse(PaiementId, out var paiementGuid))
                return;

            // 1) Paiement
            var paiement = await _paiementsApi.GetByIdAsync(paiementGuid);

            Montant = paiement.Montant;
            DatePaiement = paiement.DatePaiement;
            UserFullName = paiement.NomCompletUser;
            var userId = paiement.UserId;
            var appelId = paiement.AppelDeFondsId;

            // 2) User (rôle)
            var allUsers = await _authApi.GetAllAsync();
            var user = allUsers?.Data?.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                UserRole = user.Roles?.FirstOrDefault();
                // UserAdresse à remplir si endpoint Users/{id}
            }

            // 3) Appel de fonds
            var appel = await _appelsApi.GetByIdAsync(appelId.ToString());
            AppelDescription = appel.Description;
            MontantTotal = appel.MontantTotal;
            MontantReste = appel.MontantReste;
            NbPaiements = appel.NbPaiements;
            DateEmission = appel.DateEmission;
            ResidenceNom = appel.ResidenceNom;

            // 4) Résidence
            var residence = await _residencesApi.GetByIdAsync(appel.ResidenceId.ToString());
            ResidenceAdresseComplete =
                $"{residence.Adresse}, {residence.Ville} {residence.CodePostal}";
        }

        [RelayCommand]
        public Task GoBack()
            => Shell.Current.GoToAsync("..");
    }
}
