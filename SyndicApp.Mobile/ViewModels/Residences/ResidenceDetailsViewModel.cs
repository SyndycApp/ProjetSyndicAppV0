using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Residences;

[QueryProperty(nameof(Id), "id")]
public partial class ResidenceDetailsViewModel : ObservableObject
{
    private readonly IResidencesApi _api;

    [ObservableProperty] private string id = "";
    [ObservableProperty] private ResidenceDto? residence;

    public ResidenceDetailsViewModel(IResidencesApi api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (string.IsNullOrWhiteSpace(Id)) return;
        Residence = await _api.GetByIdAsync(Id);
    }

    [RelayCommand]
    public async Task EditAsync()
    {
        if (string.IsNullOrWhiteSpace(Id)) return;
        await Shell.Current.GoToAsync($"residence-edit?id={Id}");
    }

    [RelayCommand]
    public async Task DeleteAsync()
    {
        if (string.IsNullOrWhiteSpace(Id)) return;

        var ok = await Shell.Current.DisplayAlert(
            "Suppression", "Supprimer cette résidence ?", "Oui", "Non");
        if (!ok) return;

        try
        {
            var guid = Guid.Parse(Id);
            await _api.DeleteAsync(guid);

            WeakReferenceMessenger.Default.Send(new ResidenceChangedMessage(true));

           
            await Shell.Current.GoToAsync("//residences");
        }
        catch (ApiException ex)
        {
           
            var msg = string.IsNullOrWhiteSpace(ex.Content)
                ? ex.Message
                : ex.Content;

            await Shell.Current.DisplayAlert("Erreur API", msg, "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
        }
    }
}
