using Org.Xmlpull.V1.Sax2;
using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class AppelsPage : ContentPage
{
    private bool _isOpen;

    public AppelsPage() : this(ServiceHelper.GetRequiredService<AppelsListViewModel>()) { }

    public AppelsPage(AppelsListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        Loaded += async (_, __) => await vm.LoadAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var width = this.Width > 0 ? this.Width : Application.Current?.Windows[0]?.Page?.Width ?? 360;
        Drawer.WidthRequest = width;
        Drawer.TranslationX = -width;
        Backdrop.InputTransparent = true;
        Backdrop.Opacity = 0;
        _isOpen = false;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (width > 0)
        {
            Drawer.WidthRequest = width;
            if (!_isOpen) Drawer.TranslationX = -width;
        }
    }

    private async void OpenDrawer_Clicked(object sender, EventArgs e)
    {
        if (_isOpen) return;
        _isOpen = true;
        Backdrop.InputTransparent = false;
        await Backdrop.FadeTo(1, 160, Easing.CubicOut);
        await Drawer.TranslateTo(0, 0, 220, Easing.CubicOut);
    }

    private async void CloseDrawer_Clicked(object sender, EventArgs e) => await CloseDrawerAsync();

    private async void Backdrop_Tapped(object sender, TappedEventArgs e) => await CloseDrawerAsync();

    private async Task CloseDrawerAsync()
    {
        if (!_isOpen) return;
        _isOpen = false;
        await Drawer.TranslateTo(-Drawer.Width, 0, 220, Easing.CubicIn);
        await Backdrop.FadeTo(0, 140, Easing.CubicIn);
        Backdrop.InputTransparent = true;
    }

    private async void OnMenuItemClicked(object sender, EventArgs e)
    {
        if (sender is Button b && b.CommandParameter is string route && !string.IsNullOrWhiteSpace(route))
        {
            await CloseDrawerAsync();
            await Shell.Current.GoToAsync(route);
        }
    }
}
