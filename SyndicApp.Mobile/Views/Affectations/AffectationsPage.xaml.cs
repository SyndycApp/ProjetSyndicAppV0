using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Affectations;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationsPage : ContentPage
    {
        private bool _isOpen;

        public AffectationsPage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<AffectationsListViewModel>();

            Shell.Current.Navigating += (_, __) => ForceCloseDrawer();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var width = this.Width > 0
                ? this.Width
                : Application.Current?.Windows[0]?.Page?.Width ?? 360;

            if (Drawer != null)
            {
                Drawer.IsVisible = false;         
                Drawer.WidthRequest = width;
                Drawer.TranslationX = -width;
            }

            if (Backdrop != null)
            {
                Backdrop.InputTransparent = true;
                Backdrop.Opacity = 0;
                Backdrop.IsVisible = false;        
            }

            _isOpen = false;

            if (BindingContext is AffectationsListViewModel vm)
            {
                try { await vm.LoadAsync(); }
                catch { /* no-op */ }
            }
        }


        private async void OpenDrawer_Clicked(object sender, EventArgs e)
        {
            if (_isOpen) return;
            _isOpen = true;

            if (Drawer != null)
                Drawer.IsVisible = true;

            if (Backdrop != null)
            {
                Backdrop.IsVisible = true;
                Backdrop.InputTransparent = false;
                await Backdrop.FadeTo(1, 160, Easing.CubicOut);
            }

            // Fallback largeur
            double w = Drawer?.Width > 0
                ? Drawer.Width
                : (this.Width > 0 ? this.Width : 360);

            if (Drawer != null)
            {
                if (Drawer.WidthRequest <= 0)
                    Drawer.WidthRequest = w;

                await Drawer.TranslateTo(0, 0, 220, Easing.CubicOut);
            }
        }

        private async Task CloseDrawerAsync()
        {
            if (!_isOpen) return;
            _isOpen = false;

            if (Drawer != null)
                await Drawer.TranslateTo(-Drawer.Width, 0, 220, Easing.CubicIn);

            if (Backdrop != null)
                await Backdrop.FadeTo(0, 140, Easing.CubicIn);

            if (Backdrop != null)
            {
                Backdrop.InputTransparent = true;
                Backdrop.IsVisible = false;        
            }

            if (Drawer != null)
                Drawer.IsVisible = false;      
        }

        private void ForceCloseDrawer()
        {
            double w = Drawer?.Width > 0
                ? Drawer.Width
                : (this.Width > 0 ? this.Width : 360);

            if (Drawer != null)
            {
                if (Drawer.WidthRequest <= 0)
                    Drawer.WidthRequest = w;

                Drawer.TranslationX = -w;
            }

            if (Backdrop != null)
            {
                Backdrop.Opacity = 0;
                Backdrop.InputTransparent = true;
                Backdrop.IsVisible = false;
            }

            _isOpen = false;
        }

        private async void CloseDrawer_Clicked(object sender, EventArgs e)
            => await CloseDrawerAsync();

        private async void Backdrop_Tapped(object sender, TappedEventArgs e)
            => await CloseDrawerAsync();

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            if (sender is not Button b) return;
            if (b.CommandParameter is not string route || string.IsNullOrWhiteSpace(route)) return;

            var current = Shell.Current.CurrentState.Location?.OriginalString ?? "";
            if (string.Equals(current, route, StringComparison.OrdinalIgnoreCase))
            {
                ForceCloseDrawer();
                return;
            }

            ForceCloseDrawer();

            try
            {
                await Shell.Current.GoToAsync(route);
            }
            catch
            {
                await DisplayAlert("Navigation", $"Route introuvable : {route}", "OK");
            }
        }
    }
}
