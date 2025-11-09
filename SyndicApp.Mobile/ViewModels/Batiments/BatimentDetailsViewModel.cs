using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Batiments;

[QueryProperty(nameof(Id), "id")]
public partial class BatimentDetailsViewModel : ObservableObject
{
    private readonly IBatimentsApi _api;

    [ObservableProperty] private string id = "";
    [ObservableProperty] private BatimentDto? batiment;

    public BatimentDetailsViewModel(IBatimentsApi api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (string.IsNullOrWhiteSpace(Id)) return;
        var guid = Guid.Parse(Id);
        Batiment = await _api.GetByIdAsync(guid);
    }

    [RelayCommand]
    public Task EditAsync()
        => string.IsNullOrWhiteSpace(Id)
            ? Task.CompletedTask
            : Shell.Current.GoToAsync($"batiment-edit?id={Id}");

    [RelayCommand]
    public async Task DeleteAsync()
    {
        if (string.IsNullOrWhiteSpace(Id)) return;
        if (!await Shell.Current.DisplayAlert("Suppression", "Supprimer ce bâtiment ?", "Oui", "Non")) return;

        try { await _api.DeleteAsync(Guid.Parse(Id)); }
        catch (ApiException ex)
        {
            await Shell.Current.DisplayAlert("Erreur API", ex.Content ?? ex.Message, "OK");
            return;
        }

        WeakReferenceMessenger.Default.Send(new BatimentChangedMessage(true));
        await Shell.Current.GoToAsync("//batiments");
    }
}
