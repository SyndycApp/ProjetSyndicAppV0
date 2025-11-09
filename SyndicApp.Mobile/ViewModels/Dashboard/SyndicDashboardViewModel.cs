// ViewModels/Dashboard/SyndicDashboardViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.ViewModels.Dashboard;

public partial class SyndicDashboardViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IAccountApi _accountApi;
    private readonly IResidencesApi _residencesApi;
    private readonly TokenStore _tokenStore;
    private readonly IBatimentsApi _batimentsApi;

    

    [ObservableProperty] bool canAddResidence;
    [ObservableProperty] int batimentsCount;

    // ➜ KPI dynamiques
    [ObservableProperty] int residencesCount;   // ← lié dans le XAML

    // KPIs statiques (tu peux les brancher plus tard)
    public int LotsCount { get; } = 120;
    public int IncidentsOuverts { get; } = 3;
    public int InterventionsEnCours { get; } = 2;
    public int AppelsOuverts { get; } = 4;
    public int DocumentsCount { get; } = 56;
    public int NotificationsNonLues { get; } = 7;

    public double TauxRecouvrement { get; } = 0.72;
    public string TauxRecouvrementPct => $"{(int)(TauxRecouvrement * 100)} %";
    public double TauxResolutionIncidents { get; } = 0.86;
    public string TauxResolutionIncidentsPct => $"{(int)(TauxResolutionIncidents * 100)} %";
    public double TauxOccupation { get; } = 0.80;
    public string TauxOccupationPct => $"{(int)(TauxOccupation * 100)} %";

    // Commandes exposées (déjà utilisées par le XAML)
    public IAsyncRelayCommand GoToAppelsAsyncCommand { get; }
    public IAsyncRelayCommand GoToAppelCreateAsyncCommand { get; }
    public IAsyncRelayCommand GoToAddResidenceAsyncCommand { get; }
    public IAsyncRelayCommand GoToIncidentsAsyncCommand { get; }
    public IAsyncRelayCommand LogoutCommand { get; }
    public IAsyncRelayCommand LoadKpisAsyncCommand { get; }   // ← nouvelle

    public SyndicDashboardViewModel(IAccountApi accountApi, IResidencesApi residencesApi, IBatimentsApi batimentsApi, TokenStore tokenStore)
    {
        _accountApi = accountApi;
        _residencesApi = residencesApi;
        _tokenStore = tokenStore;
        _batimentsApi = batimentsApi;

        Title = "Dashboard";
        CanAddResidence = _tokenStore.IsSyndic();

        GoToAppelsAsyncCommand = new AsyncRelayCommand(GoToAppelsAsync);
        GoToAppelCreateAsyncCommand = new AsyncRelayCommand(GoToAppelCreateAsync);
        GoToAddResidenceAsyncCommand = new AsyncRelayCommand(GoToAddResidenceAsync);
        GoToIncidentsAsyncCommand = new AsyncRelayCommand(GoToIncidentsAsync);
        LogoutCommand = new AsyncRelayCommand(LogoutAsync);
        LoadKpisAsyncCommand = new AsyncRelayCommand(LoadKpisAsync);

       
        WeakReferenceMessenger.Default.Register<ResidenceChangedMessage>(this,
            async (_, __) => await RefreshResidencesCountAsync());
    }

    //private async Task LoadKpisAsync() => await RefreshResidencesCountAsync();
    private async Task LoadKpisAsync()
    {
        IsBusy = true;
        try
        {
            await RefreshResidencesCountAsync();
            await RefreshBatimentsCountAsync();
        }
        finally
        {
            IsBusy = false;
        }
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
        try { await Shell.Current.GoToAsync("///appels/list", true); }
        catch
        {
            try { await Shell.Current.GoToAsync("//appels", true); } catch { }
        }
    }

    private Task GoToAppelCreateAsync() => Shell.Current.GoToAsync("///appel-create");
    private Task GoToIncidentsAsync() => Shell.Current.DisplayAlert("Incidents", "Page Incidents à venir.", "OK");
}
