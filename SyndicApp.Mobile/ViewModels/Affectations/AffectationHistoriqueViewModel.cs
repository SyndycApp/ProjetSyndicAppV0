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

        [ObservableProperty] private string? lotIdParam;

        [ObservableProperty] private Guid lotId;

        [ObservableProperty] private List<AffectationLotDto> items;

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (!Guid.TryParse(LotIdParam, out var gid))
                return;

            LotId = gid;

            var data = await _api.GetHistoriqueByLotAsync(gid);
            Items = data?.ToList() ?? new List<AffectationLotDto>();
        }
    }
}
