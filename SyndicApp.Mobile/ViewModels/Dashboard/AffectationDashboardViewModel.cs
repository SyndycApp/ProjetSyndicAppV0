using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Dashboard
{
    public partial class AffectationDashboardViewModel : ObservableObject
    {
        private readonly IAffectationsLotsApi _api;
        [ObservableProperty] private bool isBusy;

        public AffectationDashboardViewModel(IAffectationsLotsApi api)
        {
            _api = api;
        }

        [ObservableProperty] private int totalLots;
        [ObservableProperty] private int actives;
        [ObservableProperty] private int closed;
        [ObservableProperty] private string occupancyRateText = "—";
        [ObservableProperty] private List<AffectationLotDto> lastItems = new();
        [ObservableProperty] private Chart? ownersVsTenantsChart;
        [ObservableProperty] private Chart? statusChart;

        [RelayCommand]
        public async Task Load()
        {
            var data = await _api.GetAllAsync() ?? new List<AffectationLotDto>();

            // KPIs
            TotalLots = data.Select(d => d.LotId).Distinct().Count();
            Actives = data.Count(d => string.Equals(d.Statut, "Active", StringComparison.OrdinalIgnoreCase));
            Closed = data.Count(d =>
                string.Equals(d.Statut, "Clôturée", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(d.Statut, "Cloturee", StringComparison.OrdinalIgnoreCase));

            var currentActiveLots = data
                .Where(d => d.DateFin == null || d.DateFin >= DateTime.Today)
                .Select(d => d.LotId)
                .Distinct()
                .Count();

            var rate = TotalLots == 0 ? 0 : (int)Math.Round(100.0 * currentActiveLots / TotalLots);
            OccupancyRateText = $"{rate}%";

            LastItems = data.OrderByDescending(d => d.DateDebut).Take(8).ToList();

            // Graphiques
            var owners = data.Count(d => d.EstProprietaire);
            var tenants = data.Count - owners;

            OwnersVsTenantsChart = new DonutChart
            {
                Entries = new[]
                {
                    new ChartEntry(owners)
                    {
                        Label = "Propriétaires",
                        ValueLabel = owners.ToString(),
                        Color = new SKColor(0x25, 0x63, 0xEB)
                    },
                    new ChartEntry(tenants)
                    {
                        Label = "Locataires",
                        ValueLabel = tenants.ToString(),
                        Color = new SKColor(0x93, 0xC5, 0xFD)
                    }
                }
            };

            StatusChart = new DonutChart
            {
                Entries = new[]
                {
                    new ChartEntry(Actives)
                    {
                        Label = "Actives",
                        ValueLabel = Actives.ToString(),
                        Color = new SKColor(0x10, 0xB9, 0x81)
                    },
                    new ChartEntry(Closed)
                    {
                        Label = "Clôturées",
                        ValueLabel = Closed.ToString(),
                        Color = new SKColor(0xEF, 0x44, 0x44)
                    }
                }
            };
        }
    }
}
