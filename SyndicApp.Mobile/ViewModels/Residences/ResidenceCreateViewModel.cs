using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Residences;

public partial class ResidenceCreateViewModel : ObservableObject
{
    private readonly IResidencesApi _api;

    [ObservableProperty] private string nom = "";
    [ObservableProperty] private string adresse = "";
    [ObservableProperty] private string ville = "";
    [ObservableProperty] private string codePostal = "";

    public ResidenceCreateViewModel(IResidencesApi api) => _api = api;

    [RelayCommand]
    public async Task CreateAsync()
    {
        var dto = new ResidenceDto
        {
            Nom = Nom,
            Adresse = Adresse,
            Ville = Ville,
            CodePostal = CodePostal
        };

        await _api.CreateAsync(dto);

        // notifier la liste puis revenir
        WeakReferenceMessenger.Default.Send(new ResidenceChangedMessage(true));
        await Shell.Current.GoToAsync("//residences");
    }

    
    [RelayCommand]
    public async Task CancelAsync() => await Shell.Current.GoToAsync("//residences");
}
