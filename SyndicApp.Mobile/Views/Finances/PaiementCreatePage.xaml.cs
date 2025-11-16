using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances
{
    public partial class PaiementCreatePage : ContentPage
    {
        private readonly PaiementCreateViewModel _viewModel;
        private bool _isFirstLoad = true;

        public PaiementCreatePage(PaiementCreateViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_isFirstLoad)
            {
                _isFirstLoad = false;
                await _viewModel.LoadAsync();
            }
        }

        // ===== Drawer =====

        private async Task OpenDrawerAsync()
        {
            Backdrop.InputTransparent = false;
            await Task.WhenAll(
                Backdrop.FadeTo(1, 150),
                Drawer.TranslateTo(0, 0, 150, Easing.CubicOut)
            );
        }

        private async Task CloseDrawerAsync()
        {
            await Task.WhenAll(
                Backdrop.FadeTo(0, 150),
                Drawer.TranslateTo(-1000, 0, 150, Easing.CubicIn)
            );
            Backdrop.InputTransparent = true;
        }

        private async void OpenDrawer_Clicked(object sender, EventArgs e)
        {
            await OpenDrawerAsync();
        }

        private async void CloseDrawer_Clicked(object sender, EventArgs e)
        {
            await CloseDrawerAsync();
        }

        private async void Backdrop_Tapped(object sender, EventArgs e)
        {
            await CloseDrawerAsync();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            if (sender is Button btn &&
                btn.CommandParameter is string route &&
                !string.IsNullOrWhiteSpace(route))
            {
                await Shell.Current.GoToAsync(route);
            }

            await CloseDrawerAsync();
        }
    }
}
