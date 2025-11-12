using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Dashboard;

public partial class AffectationAnalyticsViewModel : ObservableObject
{
    private readonly IAffectationsLotsApi _api;

    public AffectationAnalyticsViewModel(IAffectationsLotsApi api) => _api = api;

    [ObservableProperty] private Chart? createdPerMonthChart;
    [ObservableProperty] private Chart? lotRotationChart;
    [ObservableProperty] private int avgDurationDays;

    [RelayCommand]
    public async Task Load()
    {
        var all = await _api.GetAllAsync() ?? new List<AffectationLotDto>();

        // Créations par mois (12 derniers mois)
        var start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-11);
        var months = Enumerable.Range(0, 12).Select(i => start.AddMonths(i)).ToList();
        var points = months.Select(m =>
        {
            var count = all.Count(a => a.DateDebut.Year == m.Year && a.DateDebut.Month == m.Month);
            return new ChartEntry(count)
            {
                Label = m.ToString("MM/yy"),
                ValueLabel = count.ToString(),
                Color = new SKColor(0x25, 0x63, 0xEB)
            };
        }).ToArray();
        CreatedPerMonthChart = new BarChart { Entries = points };

        // Durée moyenne (jours) des affectations clôturées
        var durations = all.Where(a => a.DateFin != null)
                           .Select(a => (a.DateFin!.Value - a.DateDebut).TotalDays);
        AvgDurationDays = durations.Any() ? (int)Math.Round(durations.Average()) : 0;

        // Rotation des lots (nb d’affectations par lot sur 12 mois)
        var rotation = all.Where(a => a.DateDebut >= start)
                          .GroupBy(a => a.LotNumero ?? a.LotId.ToString())
                          .Select(g => new { Key = g.Key, C = g.Count() })
                          .OrderByDescending(x => x.C).Take(7).ToList();

        LotRotationChart = new BarChart
        {
            Entries = rotation.Select(r => new ChartEntry(r.C)
            {
                Label = r.Key,
                ValueLabel = r.C.ToString(),
                Color = new SKColor(0x10, 0xB9, 0x81)
            }).ToArray()
        };
    }
}
