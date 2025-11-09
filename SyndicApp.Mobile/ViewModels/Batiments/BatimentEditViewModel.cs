using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Batiments;

[QueryProperty(nameof(Id), "id")]
public partial class BatimentEditViewModel : ObservableObject
{
    private readonly IBatimentsApi _api;

    [ObservableProperty] string id = string.Empty;
    [ObservableProperty] string nom = string.Empty;
    [ObservableProperty] string residenceId = string.Empty;

    public BatimentEditViewModel(IBatimentsApi api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (!Guid.TryParse(Id, out var guid))
            return;

        try
        {
            var dto = await _api.GetByIdAsync(guid);
            Nom = dto.Nom ?? string.Empty;
            ResidenceId = dto.ResidenceId.ToString();
        }
        catch (ApiException ex)
        {
            await Shell.Current.DisplayAlert("Erreur API",
                string.IsNullOrWhiteSpace(ex.Content) ? ex.Message : ex.Content, "OK");
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!Guid.TryParse(Id, out var guid))
            return;

        if (string.IsNullOrWhiteSpace(Nom))
        {
            await Shell.Current.DisplayAlert("Validation", "Le nom est obligatoire.", "OK");
            return;
        }
        if (!Guid.TryParse(ResidenceId, out var residenceGuid))
        {
            await Shell.Current.DisplayAlert("Validation", "ResidenceId doit être un GUID valide.", "OK");
            return;
        }

        try
        {
            await _api.UpdateAsync(guid, new BatimentUpdateDto
            {
                Nom = Nom.Trim(),
                ResidenceId = residenceGuid
            });

            // 🔔 prévenir la liste sans changer ton flow
            WeakReferenceMessenger.Default.Send(new BatimentChangedMessage(true));

            await Shell.Current.DisplayAlert("Succès", "Bâtiment modifié.", "OK");
            await Shell.Current.GoToAsync($"batiment-details?id={guid:D}");
        }
        catch (ApiException ex)
        {
            await Shell.Current.DisplayAlert("Erreur API",
                string.IsNullOrWhiteSpace(ex.Content) ? ex.Message : ex.Content, "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private Task CancelAsync() => Shell.Current.GoToAsync("//batiments");
}
