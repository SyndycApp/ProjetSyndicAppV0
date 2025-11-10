using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Lots;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Views.Lots
{
    public partial class LotsPage : ContentPage
    {
        private bool _isOpen;

        public LotsPage() : this(ServiceHelper.Services.GetRequiredService<LotsListViewModel>()) { }
        public LotsPage(LotsListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

            // Charge la liste au premier affichage
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

        protected override void OnSizeAllocated(double w, double h)
        {
            base.OnSizeAllocated(w, h);
            if (w > 0)
            {
                Drawer.WidthRequest = w;
                if (!_isOpen) Drawer.TranslationX = -w;
            }
        }

        private async void OpenDrawer_Clicked(object s, EventArgs e)
        {
            if (_isOpen) return;
            _isOpen = true;
            Backdrop.InputTransparent = false;
            await Backdrop.FadeTo(1, 160, Easing.CubicOut);
            await Drawer.TranslateTo(0, 0, 220, Easing.CubicOut);
        }

        private async void CloseDrawer_Clicked(object s, EventArgs e) => await CloseDrawerAsync();
        private async void Backdrop_Tapped(object s, TappedEventArgs e) => await CloseDrawerAsync();

        private async Task CloseDrawerAsync()
        {
            if (!_isOpen) return;
            _isOpen = false;
            await Drawer.TranslateTo(-Drawer.Width, 0, 220, Easing.CubicIn);
            await Backdrop.FadeTo(0, 140, Easing.CubicIn);
            Backdrop.InputTransparent = true;
        }

        private async void OnMenuItemClicked(object s, EventArgs e)
        {
            if (s is Button b && b.CommandParameter is string r && !string.IsNullOrWhiteSpace(r))
            {
                await CloseDrawerAsync();
                await Shell.Current.GoToAsync(r);
            }
        }
    }
}
