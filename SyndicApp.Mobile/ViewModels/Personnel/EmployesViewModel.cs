using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Personnel;

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
        Employes = await _api.GetPersonnelInterneAsync();
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
