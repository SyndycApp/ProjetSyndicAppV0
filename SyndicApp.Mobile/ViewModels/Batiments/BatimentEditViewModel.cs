using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;

namespace SyndicApp.Mobile.ViewModels.Batiments;

[QueryProperty(nameof(Id), "id")]
public partial class BatimentEditViewModel : ObservableObject
{
    private readonly IBatimentsApi _batimentsApi;
    private readonly IResidencesApi _residencesApi;

    [ObservableProperty] string id = string.Empty;

    [ObservableProperty] string nom = string.Empty;
    [ObservableProperty] string bloc = string.Empty;
    [ObservableProperty] int nbEtages;
    [ObservableProperty] string responsableNom = string.Empty;
    [ObservableProperty] bool hasAscenseur;
    [ObservableProperty] int anneeConstruction;
    [ObservableProperty] string codeAcces = string.Empty;

    [ObservableProperty] ObservableCollection<ResidenceDto> residences = new();
    [ObservableProperty] ResidenceDto? selectedResidence;

    public BatimentEditViewModel(IBatimentsApi batimentsApi, IResidencesApi residencesApi)
    {
        _batimentsApi = batimentsApi;
        _residencesApi = residencesApi;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (!Guid.TryParse(Id, out var guid))
            return;

        // Charger résidences
        var res = await _residencesApi.GetAllAsync();
        Residences = new ObservableCollection<ResidenceDto>(res);

        // Charger bâtiment actuel
        var dto = await _batimentsApi.GetByIdAsync(guid);
        if (dto is null) return;

        Nom = dto.Nom ?? string.Empty;
        Bloc = dto.Bloc ?? string.Empty;
        NbEtages = dto.NbEtages;
        ResponsableNom = dto.ResponsableNom ?? string.Empty;
        HasAscenseur = dto.HasAscenseur;
        AnneeConstruction = dto.AnneeConstruction;
        CodeAcces = dto.CodeAcces ?? string.Empty;

        SelectedResidence = Residences.FirstOrDefault(r => r.Id == dto.ResidenceId);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!Guid.TryParse(Id, out var guid))
            return;

        if (string.IsNullOrWhiteSpace(Nom))
        {
            await Shell.Current.DisplayAlert("Validation", "Le nom du bâtiment est obligatoire.", "OK");
            return;
        }

        if (SelectedResidence is null)
        {
            await Shell.Current.DisplayAlert("Validation", "Choisis une résidence.", "OK");
            return;
        }

        try
        {
            var residenceId = await _residencesApi.LookupIdAsync(SelectedResidence.Nom!);

            await _batimentsApi.UpdateAsync(guid, new BatimentUpdateDto
            {
                Nom = Nom.Trim(),
                ResidenceId = residenceId,
                Bloc = Bloc,
                NbEtages = NbEtages,
                ResponsableNom = ResponsableNom,
                HasAscenseur = HasAscenseur,
                AnneeConstruction = AnneeConstruction,
                CodeAcces = CodeAcces
            });

            await Shell.Current.DisplayAlert("Succès", "Bâtiment modifié.", "OK");
            await Shell.Current.GoToAsync($"batiment-details?id={guid:D}");
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

    [RelayCommand]
    private Task CancelAsync() => Shell.Current.GoToAsync("//batiments");
}
