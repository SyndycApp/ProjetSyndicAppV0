using SyndicApp.Mobile.Services.AppelVocal;

namespace SyndicApp.Mobile;

public partial class App : Application
{
    public static string? UserId { get; set; }

    public App(CallHubService callHub)
    {
        InitializeComponent();

        // 🔥 LISTENER GLOBAL DES APPELS ENTRANTS
        callHub.IncomingCall += async (callId, callerId) =>
        {
            Console.WriteLine($"📞 APPEL ENTRANT GLOBAL → {callId}");
            Console.WriteLine($"🔍 Shell.Current = {Shell.Current}");
            Console.WriteLine($"🔍 MainPage = {MainPage}");

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync(
                    "incoming-call",
                    new Dictionary<string, object>
                    {
                        ["CallId"] = callId,
                        ["CallerId"] = callerId
                    }
                );
            });
        };

        Console.WriteLine("✅ IncomingCall handler global ACTIF");
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
