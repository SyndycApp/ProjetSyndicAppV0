using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Dashboard;

public partial class AffectationMaintenanceDashboardViewModel : ObservableObject
{
    private readonly IAffectationsLotsApi _api;

    public AffectationMaintenanceDashboardViewModel(IAffectationsLotsApi api) => _api = api;

    [ObservableProperty] private List<AffectationLotDto> currentOccupants = new();
    [ObservableProperty] private List<AffectationLotDto> endingSoon = new();
    [ObservableProperty] private List<AffectationLotDto> recentlyClosed = new();

    [RelayCommand]
    public async Task Load()
    {
        var all = await _api.GetAllAsync() ?? new List<AffectationLotDto>();
        var today = DateTime.Today;

        CurrentOccupants = all
            .Where(x => (x.DateDebut <= today) && (x.DateFin == null || x.DateFin >= today))
            .OrderBy(x => x.LotNumero)
            .ToList();

        EndingSoon = all
            .Where(x => x.DateFin != null && x.DateFin >= today && x.DateFin <= today.AddDays(30))
            .OrderBy(x => x.DateFin)
            .ToList();

        RecentlyClosed = all
            .Where(x => x.DateFin != null && x.DateFin >= today.AddDays(-30) && x.DateFin < today)
            .OrderByDescending(x => x.DateFin)
            .ToList();
    }
}
