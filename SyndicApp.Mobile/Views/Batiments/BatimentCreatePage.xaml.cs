using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Batiments;

namespace SyndicApp.Mobile.Views.Batiments
{
    public partial class BatimentCreatePage : ContentPage
    {
        private bool _isOpen;

        public BatimentCreatePage(BatimentCreateViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var width = this.Width;
            if (width <= 0)
                width = Application.Current?.Windows?.FirstOrDefault()?.Page?.Width ?? 360;

            Drawer.IsVisible = false;
            Drawer.WidthRequest = width;
            Drawer.TranslationX = -width;

            Backdrop.InputTransparent = true;
            Backdrop.Opacity = 0;
            _isOpen = false;

            // ✅ Charge la liste des résidences pour le Picker
            if (BindingContext is BatimentCreateViewModel vm)
                await vm.LoadResidencesAsync();
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
            Drawer.IsVisible = true;
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
            Drawer.IsVisible = false;
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
}
