using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyndicApp.Mobile.ViewModels.Dashboard;

public partial class SyndicDashboardViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IAccountApi _accountApi;
    private readonly TokenStore _tokenStore;

    [ObservableProperty] bool canAddResidence;

    // ⚠️ Exposées explicitement pour que le XAML les voie toujours
    public IAsyncRelayCommand GoToAppelsAsyncCommand { get; }
    public IAsyncRelayCommand GoToAppelCreateAsyncCommand { get; }
    public IAsyncRelayCommand GoToAddResidenceAsyncCommand { get; }
    public IAsyncRelayCommand GoToIncidentsAsyncCommand { get; } // ← manquante avant
    public IAsyncRelayCommand LogoutCommand { get; }


    // KPIs numériques
    public int ResidencesCount { get; } = 8;
    public int LotsCount { get; } = 120;
    public int OccupantsCount { get; } = 95;
    public int IncidentsOuverts { get; } = 3;
    public int InterventionsEnCours { get; } = 2;
    public int AppelsOuverts { get; } = 4;
    public int DocumentsCount { get; } = 56;
    public int NotificationsNonLues { get; } = 7;

    // KPIs pourcentages (0..1) + formats "72 %"
    public double TauxRecouvrement { get; } = 0.72;
    public string TauxRecouvrementPct => $"{(int)(TauxRecouvrement * 100)} %";

    public double TauxResolutionIncidents { get; } = 0.86;
    public string TauxResolutionIncidentsPct => $"{(int)(TauxResolutionIncidents * 100)} %";

    public double TauxOccupation { get; } = 0.80;
    public string TauxOccupationPct => $"{(int)(TauxOccupation * 100)} %";

    public SyndicDashboardViewModel(IAccountApi accountApi, TokenStore tokenStore)
    {
        _accountApi = accountApi;
        _tokenStore = tokenStore;
        Title = "Dashboard";
        CanAddResidence = _tokenStore.IsSyndic();

        GoToAppelsAsyncCommand = new AsyncRelayCommand(GoToAppelsAsync);
        GoToAppelCreateAsyncCommand = new AsyncRelayCommand(GoToAppelCreateAsync);
        GoToAddResidenceAsyncCommand = new AsyncRelayCommand(GoToAddResidenceAsync);
        GoToIncidentsAsyncCommand = new AsyncRelayCommand(GoToIncidentsAsync); // ← ajout
        LogoutCommand = new AsyncRelayCommand(LogoutAsync);
    }

    private async Task LogoutAsync()
    {
        var ok = await Shell.Current.DisplayAlert("Déconnexion", "Voulez-vous vous déconnecter ?", "Oui", "Non");
        if (!ok) return;

        try { await _accountApi.LogoutAsync(); } catch { /* ignore */ }

        _tokenStore.Clear();
        await Shell.Current.GoToAsync("//login");
        await Shell.Current.DisplayAlert("Déconnexion", "À bientôt !", "OK");
    }

    private Task GoToAddResidenceAsync()
    {
        if (!CanAddResidence) return Task.CompletedTask;
        return Shell.Current.GoToAsync("residences/add");
    }

    private async Task GoToAppelsAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("///appels/list", true);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Navigation", $"Erreur: {ex.Message}", "OK");
            // Secours: bascule sur le ShellContent "appels" (2 slash)
            try { await Shell.Current.GoToAsync("//appels", true); } catch { /* ignore */ }
        }
    }


    private Task GoToAppelCreateAsync() => Shell.Current.GoToAsync("///appel-create");


    // Placeholder incidents (à remplacer quand la page existe)
    private async Task GoToIncidentsAsync()
        => await Shell.Current.DisplayAlert("Incidents", "Page Incidents à venir.", "OK");
}
