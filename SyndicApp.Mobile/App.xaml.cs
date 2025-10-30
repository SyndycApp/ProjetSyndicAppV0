using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        // Ne plus définir MainPage ici
        // MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window { Page = new AppShell() };
    }
}
