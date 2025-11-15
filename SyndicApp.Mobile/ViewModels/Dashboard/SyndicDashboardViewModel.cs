// ViewModels/Dashboard/SyndicDashboardViewModel.cs
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.ViewModels.Dashboard;

public partial class SyndicDashboardViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IAccountApi _accountApi;
    private readonly IResidencesApi _residencesApi;
    private readonly IBatimentsApi _batimentsApi;
    private readonly ILotsApi _lotsApi;
    private readonly IAppelsApi _appelsApi;
    private readonly IChargesApi _chargesApi;
    private readonly TokenStore _tokenStore;

    [ObservableProperty] bool canAddResidence;
    [ObservableProperty] int batimentsCount;
    [ObservableProperty] int residencesCount;
    [ObservableProperty] int lotsCount;

    // appels / charges dynamiques
    [ObservableProperty] int appelsOuverts;
    [ObservableProperty] int chargesCount;
    [ObservableProperty] decimal chargesMontantTotal;

    // autres KPI (toujours statiques pour l’instant)
    public int IncidentsOuverts { get; } = 3;
    public int InterventionsEnCours { get; } = 2;
    public int DocumentsCount { get; } = 56;
    public int NotificationsNonLues { get; } = 7;

    [ObservableProperty] double tauxRecouvrement;
    [ObservableProperty] string tauxRecouvrementPct = "0 %";

    partial void OnTauxRecouvrementChanged(double value)
        => TauxRecouvrementPct = $"{(int)(value * 100)} %";

    public double TauxResolutionIncidents { get; } = 0.86;
    public string TauxResolutionIncidentsPct => $"{(int)(TauxResolutionIncidents * 100)} %";
    public double TauxOccupation { get; } = 0.80;
    public string TauxOccupationPct => $"{(int)(TauxOccupation * 100)} %";

    public IAsyncRelayCommand GoToAppelsAsyncCommand { get; }
    public IAsyncRelayCommand GoToAppelCreateAsyncCommand { get; }
    public IAsyncRelayCommand GoToAddResidenceAsyncCommand { get; }
    public IAsyncRelayCommand GoToIncidentsAsyncCommand { get; }
    public IAsyncRelayCommand LogoutCommand { get; }
    public IAsyncRelayCommand LoadKpisAsyncCommand { get; }

    public SyndicDashboardViewModel(
        IAccountApi accountApi,
        IResidencesApi residencesApi,
        IBatimentsApi batimentsApi,
        ILotsApi lotsApi,
        IAppelsApi appelsApi,
        IChargesApi chargesApi,
        TokenStore tokenStore)
    {
        _accountApi = accountApi;
        _residencesApi = residencesApi;
        _batimentsApi = batimentsApi;
        _lotsApi = lotsApi;
        _appelsApi = appelsApi;
        _chargesApi = chargesApi;
        _tokenStore = tokenStore;

        Title = "Dashboard";
        CanAddResidence = _tokenStore.IsSyndic();

        GoToAppelsAsyncCommand = new AsyncRelayCommand(GoToAppelsAsync);
        GoToAppelCreateAsyncCommand = new AsyncRelayCommand(GoToAppelCreateAsync);
        GoToAddResidenceAsyncCommand = new AsyncRelayCommand(GoToAddResidenceAsync);
        GoToIncidentsAsyncCommand = new AsyncRelayCommand(GoToIncidentsAsync);
        LogoutCommand = new AsyncRelayCommand(LogoutAsync);
        LoadKpisAsyncCommand = new AsyncRelayCommand(LoadKpisAsync);

        WeakReferenceMessenger.Default.Register<ResidenceChangedMessage>(this, async (_, __) => await RefreshResidencesCountAsync());
        WeakReferenceMessenger.Default.Register<BatimentChangedMessage>(this, async (_, __) => await RefreshBatimentsCountAsync());
        WeakReferenceMessenger.Default.Register<LotChangedMessage>(this, async (_, __) => await RefreshLotsCountAsync());
    }

    private async Task LoadKpisAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        try
        {
            await RefreshResidencesCountAsync();
            await RefreshBatimentsCountAsync();
            await RefreshLotsCountAsync();
            await RefreshFinancesAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task RefreshLotsCountAsync()
    {
        try { var lots = await _lotsApi.GetAllAsync(); LotsCount = lots?.Count ?? 0; }
        catch { LotsCount = 0; }
    }

    private async Task RefreshResidencesCountAsync()
    {
        try
        {
            var all = await _residencesApi.GetAllAsync();
            ResidencesCount = all?.Count() ?? 0;
        }
        catch
        {
            ResidencesCount = 0;
        }
    }

    private async Task RefreshBatimentsCountAsync()
    {
        try
        {
            var bats = await _batimentsApi.GetAllAsync();
            BatimentsCount = bats?.Count ?? 0;
        }
        catch
        {
            BatimentsCount = 0;
        }
    }

    private async Task RefreshFinancesAsync()
    {
        try
        {
            // Appels
            var appels = await _appelsApi.GetAllAsync() ?? new System.Collections.Generic.List<AppelDeFondsDto>();
            AppelsOuverts = appels.Count;

            var totalAppels = appels.Sum(a => a.MontantTotal);
            var totalPaye = appels.Sum(a => a.MontantPaye);
            TauxRecouvrement = totalAppels > 0 ? (double)(totalPaye / totalAppels) : 0.0;

            // Charges
            var charges = await _chargesApi.GetAllAsync() ?? new System.Collections.Generic.List<ChargeDto>();
            ChargesCount = charges.Count;
            ChargesMontantTotal = charges.Sum(c => c.Montant);
        }
        catch
        {
            AppelsOuverts = 0;
            TauxRecouvrement = 0.0;
            ChargesCount = 0;
            ChargesMontantTotal = 0m;
        }
    }

    private async Task LogoutAsync()
    {
        var ok = await Shell.Current.DisplayAlert("Déconnexion", "Voulez-vous vous déconnecter ?", "Oui", "Non");
        if (!ok) return;
        try { await _accountApi.LogoutAsync(); } catch { }
        _tokenStore.Clear();
        await Shell.Current.GoToAsync("//login");
        await Shell.Current.DisplayAlert("Déconnexion", "À bientôt !", "OK");
    }

    private Task GoToAddResidenceAsync()
        => !CanAddResidence ? Task.CompletedTask : Shell.Current.GoToAsync("residences/add");

    private async Task GoToAppelsAsync()
    {
        try { await Shell.Current.GoToAsync("//appels", true); }
        catch { }
    }

    private Task GoToAppelCreateAsync() => Shell.Current.GoToAsync("appel-create");
    private Task GoToIncidentsAsync() => Shell.Current.DisplayAlert("Incidents", "Page Incidents à venir.", "OK");
}
