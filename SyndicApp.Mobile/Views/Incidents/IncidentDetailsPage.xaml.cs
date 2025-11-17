using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class IncidentDetailsPage : ContentPage
    {
        private bool _isDrawerOpen;

        public IncidentDetailsPage(IncidentDetailsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is IncidentDetailsViewModel vm)
            {
                await vm.LoadAsync();
            }
        }

        // ===== Drawer =====

        private async Task OpenDrawerAsync()
        {
            _isDrawerOpen = true;
            Backdrop.InputTransparent = false;

            await Task.WhenAll(
                Drawer.TranslateTo(0, 0, 250, Easing.CubicOut),
                Backdrop.FadeTo(1, 250, Easing.CubicIn)
            );
        }

        private async Task CloseDrawerAsync()
        {
            if (!_isDrawerOpen)
                return;

            _isDrawerOpen = false;
            Backdrop.InputTransparent = true;

            await Task.WhenAll(
                Drawer.TranslateTo(-1000, 0, 250, Easing.CubicIn),
                Backdrop.FadeTo(0, 250, Easing.CubicOut)
            );
        }

        private async void OpenDrawer_Clicked(object sender, EventArgs e)
        {
            await OpenDrawerAsync();
        }

        private async void CloseDrawer_Clicked(object sender, EventArgs e)
        {
            await CloseDrawerAsync();
        }

        private async void Backdrop_Tapped(object sender, TappedEventArgs e)
        {
            await CloseDrawerAsync();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            if (sender is Button btn &&
                btn.CommandParameter is string route &&
                !string.IsNullOrWhiteSpace(route))
            {
                await CloseDrawerAsync();
                await Shell.Current.GoToAsync(route);
            }
        }
    }
}
