using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

public partial class EmployesViewModel : ObservableObject
{
    private readonly IPersonnelApi _api;

    [ObservableProperty]
    private List<PersonnelLookupDto> employes = new();

    [ObservableProperty]
    private PersonnelLookupDto? selectedEmploye;

    public EmployesViewModel(IPersonnelApi api)
    {
        _api = api;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        try
        {
            Employes = await _api.GetPersonnelInterneAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await Application.Current.MainPage.DisplayAlert(
                "Erreur",
                "Impossible de charger la liste des employés",
                "OK"
            );

            Employes = new List<PersonnelLookupDto>();
        }
    }


    // ✅ SIGNATURE OBLIGATOIRE
    [RelayCommand]
    private async Task OpenDetailsAsync(SelectionChangedEventArgs args)
    {
        var employe = args?.CurrentSelection?.FirstOrDefault() as PersonnelLookupDto;

        if (employe == null)
            return;

        SelectedEmploye = employe;

        await Shell.Current.GoToAsync(
            "personnel/employe-details",
            new Dictionary<string, object>
            {
                ["userId"] = employe.UserId
            }
        );

        // ✅ évite le bug du 2e clic
        SelectedEmploye = null;
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            return;

        await Shell.Current.GoToAsync(
            "personnel/employe-details",
            new Dictionary<string, object>
            {
                ["userId"] = userId
            }
        );
    }

    [RelayCommand]
    private async Task OpenPlanningAsync()
    {
        if (SelectedEmploye == null)
            return;

        await Shell.Current.GoToAsync(
            $"//personnel/planning?userId={SelectedEmploye.UserId}&name={SelectedEmploye.FullName}"
        );
    }
}
