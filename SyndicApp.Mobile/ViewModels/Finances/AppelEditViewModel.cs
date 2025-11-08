using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances;

[QueryProperty(nameof(Id), "id")]
public partial class AppelEditViewModel : ObservableObject
{
    private readonly IAppelsApi _api;

    [ObservableProperty] private string id = string.Empty; // Guid -> string
    [ObservableProperty] private string? description;
    [ObservableProperty] private decimal montantTotal;
    [ObservableProperty] private DateTime dateEmission;
    [ObservableProperty] private string residenceId = string.Empty; // Guid -> string

    [ObservableProperty] private int nbPaiements;
    [ObservableProperty] private decimal montantPaye;
    [ObservableProperty] private decimal montantReste;

    [ObservableProperty] private bool isBusy;

    public AppelEditViewModel(IAppelsApi api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(Id)) return;
        try
        {
            IsBusy = true;
            var dto = await _api.GetByIdAsync(Id); // string
            Description = dto.Description;
            MontantTotal = dto.MontantTotal;
            DateEmission = dto.DateEmission;
            ResidenceId = dto.ResidenceId; // string
            NbPaiements = dto.NbPaiements;
            MontantPaye = dto.MontantPaye;
            MontantReste = dto.MontantReste;
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(Id)) return;
        try
        {
            IsBusy = true;

            var payload = new AppelDeFondsDto
            {
                Id = Id,                // string
                Description = Description,
                MontantTotal = MontantTotal,
                DateEmission = DateEmission == default ? DateTime.Today : DateEmission,
                ResidenceId = ResidenceId        // string
            };

            await _api.UpdateAsync(Id, payload);  // string
            await Shell.Current.GoToAsync($"appel-details?id={Id}");
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    public async Task CloturerAsync()
    {
        if (string.IsNullOrWhiteSpace(Id)) return;
        await _api.CloturerAsync(Id);             // string
        await Shell.Current.GoToAsync($"appel-details?id={Id}");
    }

    [RelayCommand]
    public async Task DeleteAsync()
    {
        if (string.IsNullOrWhiteSpace(Id)) return;

        var ok = await Shell.Current.DisplayAlert("Suppression", "Supprimer cet appel ?", "Oui", "Non");
        if (!ok) return;

        try
        {
            await _api.DeleteAsync(Id); // DELETE /api/Appels/{id}
            await Shell.Current.DisplayAlert("Succès", "Appel supprimé.", "OK");
            await Shell.Current.GoToAsync("//appels");
        }
        catch (ApiException ex)
        {
            await Shell.Current.DisplayAlert("Erreur API", ex.Content ?? ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
        }
    }
}
