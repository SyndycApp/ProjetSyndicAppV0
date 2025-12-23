using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Services.AppelVocal;

namespace SyndicApp.Mobile.ViewModels.AppelVocal;

public partial class IncomingCallViewModel : ObservableObject
{
    private readonly CallHubService _callHub;

    [ObservableProperty]
    private Guid callId;

    [ObservableProperty]
    private Guid callerId;

    public IncomingCallViewModel(CallHubService callHub)
    {
        _callHub = callHub;
        Console.WriteLine("📲 IncomingCallViewModel créé");
    }

    [RelayCommand]
    private async Task Accept()
    {
        Console.WriteLine($"✅ Accept call {CallId}");

        await _callHub.AcceptCall(CallId);

        await Shell.Current.GoToAsync("active-call",
            new Dictionary<string, object>
            {
                ["CallId"] = CallId
            });
    }

    [RelayCommand]
    private async Task Reject()
    {
        Console.WriteLine($"❌ Reject call {CallId}");

        await _callHub.EndCall(CallId);
        await Shell.Current.GoToAsync("..");
    }
}
