using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.ViewModels.Dashboard
{
    public partial class AffectationUserDashboardViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;
        private readonly TokenStore _token; // DI

        public AffectationUserDashboardViewModel(IAffectationsLotsApi api, TokenStore tokenStore)
        {
            _api = api;
            _token = tokenStore;
        }

        [ObservableProperty] private int myLotsCount;
        [ObservableProperty] private int myActives;
        [ObservableProperty] private string? nextEndingText;
        [ObservableProperty] private List<AffectationLotDto> myItems = new();

        [RelayCommand]
        public async Task Load()
        {
            var userIdStr = _token.GetUserId();
            Guid? userId = Guid.TryParse(userIdStr, out var g) ? g : null;

            var all = await _api.GetAllAsync() ?? new List<AffectationLotDto>();
            var mine = (userId is null)
                ? new List<AffectationLotDto>()
                : all.Where(x => x.UserId == userId.Value).ToList();


            MyItems = mine.OrderByDescending(x => x.DateDebut).ToList();
            MyLotsCount = mine.Select(x => x.LotId).Distinct().Count();
            MyActives = mine.Count(x => string.Equals(x.Statut, "Active", StringComparison.OrdinalIgnoreCase));

            var next = mine.Where(x => x.DateFin != null && x.DateFin >= DateTime.Today)
                           .OrderBy(x => x.DateFin).FirstOrDefault();
            NextEndingText = next?.DateFin?.ToString("dd/MM/yyyy") ?? "—";
        }
    }
}
