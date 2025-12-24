using SyndicApp.Mobile.Services.AppelVocal;

namespace SyndicApp.Mobile;

public partial class App : Application
{
    public static string? UserId { get; set; }

    public App(CallHubService callHub)
    {
        InitializeComponent();

        // 🔥 LISTENER GLOBAL UNIQUE
        callHub.IncomingCall += async (callId, callerId) =>
        {
            Console.WriteLine($"📞 APPEL ENTRANT → {callId}");

            await MainThread.InvokeOnMainThreadAsync(() =>
                Shell.Current.GoToAsync("incoming-call",
                    new Dictionary<string, object>
                    {
                        ["CallId"] = callId,
                        ["CallerId"] = callerId
                    })
            );
        };
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
