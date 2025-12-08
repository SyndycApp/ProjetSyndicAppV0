namespace SyndicApp.Mobile;

public partial class App : Application
{
    public static string? UserId { get; set; }
    public App()
    {
        InitializeComponent();

        Microsoft.Maui.Controls.Application.Current.Dispatcher.Dispatch(() =>
        {
            Console.WriteLine("🔵 App.UserId = " + UserId);
        });
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        
        var window = new Window(new AppShell());

        
        window.Created += (s, e) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//login");
            });
        };

        return window;
    }
}
