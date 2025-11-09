using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Batiments;

namespace SyndicApp.Mobile.Views.Batiments
{
    public partial class BatimentEditPage : ContentPage
    {
        private bool _isOpen;

        public BatimentEditPage(BatimentEditViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            Loaded += async (_, __) => await vm.LoadAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var width = this.Width > 0 ? this.Width : Application.Current?.Windows[0]?.Page?.Width ?? 360;

            Drawer.IsVisible = false;
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
            Drawer.IsVisible = true;
            Backdrop.InputTransparent = false;
            await Backdrop.FadeTo(1, 160, Easing.CubicOut);
            await Drawer.TranslateTo(0, 0, 220, Easing.CubicOut);
        }

        private async Task CloseDrawerAsync()
        {
            if (!_isOpen) return;
            _isOpen = false;
            await Drawer.TranslateTo(-Drawer.Width, 0, 220, Easing.CubicIn);
            await Backdrop.FadeTo(0, 140, Easing.CubicIn);
            Backdrop.InputTransparent = true;
            Drawer.IsVisible = false;
        }

        private async void CloseDrawer_Clicked(object sender, EventArgs e) => await CloseDrawerAsync();
        private async void Backdrop_Tapped(object sender, TappedEventArgs e) => await CloseDrawerAsync();
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
