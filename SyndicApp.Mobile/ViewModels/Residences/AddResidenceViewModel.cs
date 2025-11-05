using Android.Locations;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Residences;

public partial class AddResidenceViewModel : ViewModels.Common.BaseViewModel
{
    private readonly IResidencesApi _resApi;

    [ObservableProperty] string? nom;
    [ObservableProperty] string? adresse;
    [ObservableProperty] string? ville;
    [ObservableProperty] string? codePostal;
    [ObservableProperty] string? message;

    public AddResidenceViewModel(IResidencesApi resApi)
    {
        _resApi = resApi;
        Title = "Nouvelle résidence";
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        try
        {
            IsBusy = true;
            var dto = new ResidenceCreateDto
            {
                Nom = Nom,
                Adresse = Adresse,
                Ville = Ville,
                CodePostal = CodePostal
            };
            var created = await _resApi.CreateAsync(dto);
            Message = $"Résidence « {created.Nom} » créée.";
            await Shell.Current.DisplayAlert("Succès", Message, "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (ApiException ex)
        {
            await Shell.Current.DisplayAlert("Erreur", "Création impossible.", "OK");
            System.Diagnostics.Debug.WriteLine(ex);
        }
        finally { IsBusy = false; }
    }
}
