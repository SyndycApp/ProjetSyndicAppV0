using CommunityToolkit.Mvvm.ComponentModel;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Personnel;

[QueryProperty(nameof(UserId), "userId")]
[QueryProperty(nameof(UserName), "name")]
public partial class PlanningPresenceViewModel : ObservableObject
{
    private readonly IPersonnelApi _api;

    [ObservableProperty] private Guid userId;
    [ObservableProperty] private string userName = string.Empty;

    [ObservableProperty] private List<PlanningDto> planning = new();
    [ObservableProperty] private List<PresenceDto> presences = new();

    public PlanningPresenceViewModel(IPersonnelApi api)
    {
        _api = api;
    }

    public async Task LoadAsync()
    {
        // 🔒 PROTECTION CRITIQUE
        if (UserId == Guid.Empty)
            return;

        Planning = await _api.GetPlanningAsync(UserId);
        Presences = await _api.GetPresencesAsync(UserId);
    }
}
