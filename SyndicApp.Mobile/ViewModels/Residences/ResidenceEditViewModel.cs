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
        Nom = dto.Nom;
        Adresse = dto.Adresse;
        Ville = dto.Ville;
        CodePostal = dto.CodePostal;
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        // ⚠️ utiliser le DTO de mise à jour
        var payload = new ResidenceDto
        {
            Nom = Nom,
            Adresse = Adresse,
            Ville = Ville,
            CodePostal = CodePostal
        };

        await _api.UpdateAsync(Id, payload);

        // notifier la liste + rester fluide
        WeakReferenceMessenger.Default.Send(new ResidenceChangedMessage(true));
        await Shell.Current.GoToAsync($"residence-details?id={Id}");
    }
}
