using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Residences;

[QueryProperty(nameof(Id), "id")]
public partial class ResidenceEditViewModel : ObservableObject
{
    private readonly IResidencesApi _api;

    [ObservableProperty] private string id = "";
    [ObservableProperty] private string nom = "";
    [ObservableProperty] private string adresse = "";
    [ObservableProperty] private string ville = "";
    [ObservableProperty] private string codePostal = "";

    public ResidenceEditViewModel(IResidencesApi api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (string.IsNullOrWhiteSpace(Id)) return;
        var dto = await _api.GetByIdAsync(Id);

        Nom = dto.Nom ?? string.Empty;
        Adresse = dto.Adresse ?? string.Empty;
        Ville = dto.Ville ?? string.Empty;
        CodePostal = dto.CodePostal ?? string.Empty;
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        var payload = new ResidenceDto
        {
            Nom = Nom,
            Adresse = Adresse,
            Ville = Ville,
            CodePostal = CodePostal
        };

        await _api.UpdateAsync(Id, payload);

        WeakReferenceMessenger.Default.Send(new ResidenceChangedMessage(true));
        await Shell.Current.GoToAsync($"residence-details?id={Id}");
    }
}
