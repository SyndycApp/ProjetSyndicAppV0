using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Dashboard;
using System;

namespace SyndicApp.Mobile.Views.Dashboard
{
    public partial class AffectationMaintenanceDashboardPage : ContentPage
    {
        private bool _isOpen;

        public AffectationMaintenanceDashboardPage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<AffectationMaintenanceDashboardViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var width = this.Width > 0 ? this.Width : Application.Current?.Windows[0]?.Page?.Width ?? 360;
            Drawer.WidthRequest = width;
            Drawer.TranslationX = -width;
            Backdrop.InputTransparent = true;
            Backdrop.Opacity = 0;
            _isOpen = false;

            if (BindingContext is AffectationMaintenanceDashboardViewModel vm)
                await vm.Load();
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

        private async System.Threading.Tasks.Task CloseDrawerAsync()
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
}
