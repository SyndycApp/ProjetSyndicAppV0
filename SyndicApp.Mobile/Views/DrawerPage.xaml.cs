using Microsoft.Maui.Controls;
using Org.Xmlpull.V1.Sax2;
using System;

namespace SyndicApp.Mobile.Views
{
    public partial class DrawerPage : ContentPage
    {
        private bool _isOpen;

        public DrawerPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Ajuster la largeur du drawer à celle de l'écran
            var width = this.Width > 0 ? this.Width : Application.Current?.Windows[0]?.Page?.Width ?? 360;
            Drawer.WidthRequest = width;

            // Cacher le drawer à gauche au départ
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
                if (!_isOpen)
                    Drawer.TranslationX = -width;
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

        private async void CloseDrawer_Clicked(object sender, EventArgs e)
        {
            await CloseDrawerAsync();
        }

        private async void Backdrop_Tapped(object sender, TappedEventArgs e)
        {
            await CloseDrawerAsync();
        }

        private async System.Threading.Tasks.Task CloseDrawerAsync()
        {
            if (!_isOpen) return;
            _isOpen = false;

            await Drawer.TranslateTo(-Drawer.Width, 0, 220, Easing.CubicIn);
            await Backdrop.FadeTo(0, 140, Easing.CubicIn);
            Backdrop.InputTransparent = true;
        }

        // Navigation Shell pour les boutons
        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            if (sender is Button b && b.CommandParameter is string route && !string.IsNullOrWhiteSpace(route))
            {
                await CloseDrawerAsync(); // referme le menu avant navigation
                await Shell.Current.GoToAsync(route);
            }
        }
    }
}
