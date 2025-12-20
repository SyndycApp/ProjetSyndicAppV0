using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Services.AppelVocal;

public partial class IncomingCallViewModel : ObservableObject
{
    private readonly CallHubService _callHub;

    [ObservableProperty] Guid callId;
    [ObservableProperty] Guid callerId;

    public IncomingCallViewModel(CallHubService callHub)
    {
        _callHub = callHub;
    }

    [RelayCommand]
    private async Task Accept()
    {
        await _callHub.AcceptCall(CallId);

        await Shell.Current.GoToAsync("active-call", new Dictionary<string, object>
        {
            ["CallId"] = CallId,
            ["OtherUserName"] = "Appel entrant"
        });
    }

    [RelayCommand]
    private async Task Reject()
    {
        await _callHub.EndCall(CallId);
        await Shell.Current.GoToAsync("..");
    }
}
