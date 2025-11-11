using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Affectations
{
    // ⬇️ Recevoir le paramètre de route "lotId" en string
    [QueryProperty(nameof(LotIdParam), "lotId")]
    public partial class AffectationHistoriqueViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;

        public AffectationHistoriqueViewModel(IAffectationsLotsApi api)
        {
            _api = api;
            Items = new();
        }

        // Param brut reçu via Shell
        [ObservableProperty] private string? lotIdParam;

        // Optionnel: garder le Guid parsé si tu veux l’exposer
        [ObservableProperty] private Guid lotId;

        // On reste sur ton modèle unique
        [ObservableProperty] private List<AffectationLotDto> items;

        [RelayCommand]
        public async Task LoadAsync()
        {
            // ✅ Parse sécurisé
            if (!Guid.TryParse(LotIdParam, out var gid))
                return;

            LotId = gid;

            var data = await _api.GetHistoriqueByLotAsync(gid);
            Items = data?.ToList() ?? new List<AffectationLotDto>();
        }
    }
}
