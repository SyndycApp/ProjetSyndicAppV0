using CommunityToolkit.Mvvm.ComponentModel;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Personnel;

[QueryProperty(nameof(UserId), "userId")]
public partial class EmployeDetailsViewModel : ObservableObject
{
    private readonly IPersonnelApi _api;

    [ObservableProperty]
    private Guid userId;

    [ObservableProperty]
    private bool isLoaded;

    [ObservableProperty]
    private EmployeDetailsDto employe = new();

    public EmployeDetailsViewModel(IPersonnelApi api)
    {
        _api = api;
    }

    public async Task LoadAsync()
    {
        if (IsLoaded || UserId == Guid.Empty)
            return;

        IsLoaded = true;
        Employe = await _api.GetEmployeDetailsAsync(UserId);
    }

}
