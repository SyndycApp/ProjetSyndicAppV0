using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        // NE PAS toucher à MainPage ici
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var shell = new AppShell();
        var window = new Window(shell);

        // Naviguer vers //login au premier affichage
        window.Activated += (s, e) =>
        {
            shell.Dispatcher.Dispatch(async () =>
            {
                try
                {
                    await Shell.Current.GoToAsync("//login", false);
                }
                catch
                {
                    // ignore si déjà positionné
                }
            });
        };

        return window;
    }
}
