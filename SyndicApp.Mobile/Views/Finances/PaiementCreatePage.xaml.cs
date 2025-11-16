using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances
{
    public partial class PaiementCreatePage : ContentPage
    {
        private bool _isDrawerOpen;

        public PaiementCreatePage(PaiementCreateViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PaiementCreateViewModel vm)
                await vm.LoadAsync();
        }

        private async Task OpenDrawerAsync()
        {
            if (_isDrawerOpen)
                return;

            _isDrawerOpen = true;
            Backdrop.InputTransparent = false;

            if (Drawer.TranslationX == 0)
                Drawer.TranslationX = -Drawer.Width;

            await Task.WhenAll(
                Drawer.TranslateTo(0, 0, 250, Easing.CubicOut),
                Backdrop.FadeTo(1, 250, Easing.CubicOut)
            );
        }

        private async Task CloseDrawerAsync()
        {
            if (!_isDrawerOpen)
                return;

            _isDrawerOpen = false;
            Backdrop.InputTransparent = true;

            await Task.WhenAll(
                Drawer.TranslateTo(-Drawer.Width, 0, 250, Easing.CubicIn),
                Backdrop.FadeTo(0, 250, Easing.CubicIn)
            );
        }

        private async void OpenDrawer_Clicked(object sender, EventArgs e)
            => await OpenDrawerAsync();

        private async void CloseDrawer_Clicked(object sender, EventArgs e)
            => await CloseDrawerAsync();

        private async void Backdrop_Tapped(object sender, EventArgs e)
            => await CloseDrawerAsync();

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is string route && !string.IsNullOrWhiteSpace(route))
            {
                await Shell.Current.GoToAsync(route);
            }

            await CloseDrawerAsync();
        }
    }
}
