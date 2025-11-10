using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Affectations
{
    [QueryProperty(nameof(LotId), "lotId")]
    public partial class AffectationHistoriqueViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;

        public AffectationHistoriqueViewModel(IAffectationsLotsApi api)
        {
            _api = api;
            Items = new();
        }

        [ObservableProperty] private Guid lotId;
        // On reste sur ton modèle unique
        [ObservableProperty] private List<AffectationLotDto> items;

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (LotId == Guid.Empty) return;
            var data = await _api.GetHistoriqueByLotAsync(LotId);
            Items = data?.ToList() ?? new List<AffectationLotDto>();
        }
    }
}
