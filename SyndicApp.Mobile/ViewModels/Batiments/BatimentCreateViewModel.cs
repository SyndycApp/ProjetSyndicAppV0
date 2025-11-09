using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IntelliJ.Lang.Annotations;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;
using System.Collections.ObjectModel;

namespace SyndicApp.Mobile.ViewModels.Batiments;

public partial class BatimentCreateViewModel : ObservableObject
{
    private readonly IBatimentsApi _batimentsApi;
    private readonly IResidencesApi _residencesApi;

    [ObservableProperty] string nom = string.Empty;

    [ObservableProperty] ObservableCollection<ResidenceDto> residences = new();
    [ObservableProperty] ResidenceDto? selectedResidence;

    [ObservableProperty] bool isBusy;

    public BatimentCreateViewModel(IBatimentsApi batimentsApi, IResidencesApi residencesApi)
    {
        _batimentsApi = batimentsApi;
        _residencesApi = residencesApi;
    }

    [RelayCommand]
    public async Task LoadResidencesAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var list = await _residencesApi.GetAllAsync();
            Residences = new ObservableCollection<ResidenceDto>(list);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CreateAsync()
    {
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
            // 1) On résout l'Id par le nom (nouvelle API)
            var residenceId = await _residencesApi.LookupIdAsync(SelectedResidence.Nom!);

            // 2) On crée le bâtiment
            await _batimentsApi.CreateAsync(new BatimentCreateDto
            {
                Nom = Nom.Trim(),
                ResidenceId = residenceId
            });

            WeakReferenceMessenger.Default.Send(new BatimentChangedMessage(true));
            await Shell.Current.DisplayAlert("Succès", "Bâtiment créé.", "OK");
            await Shell.Current.GoToAsync("//batiments");
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
