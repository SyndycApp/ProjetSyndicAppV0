using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class DevisTravauxDetailsPage : ContentPage
    {
        private readonly DevisTravauxDetailsViewModel _viewModel;
        private bool _isDrawerOpen;

        public DevisTravauxDetailsPage(DevisTravauxDetailsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await _viewModel.LoadAsync();
            }
            catch
            {
                // déjà géré dans le ViewModel
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await _viewModel.GoBackAsync();
            }
            catch
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OpenDrawer_Clicked(object sender, EventArgs e)
        {
            await ShowDrawerAsync(true);
        }

        private async void CloseDrawer_Clicked(object sender, EventArgs e)
        {
            await ShowDrawerAsync(false);
        }

        private async void Backdrop_Tapped(object sender, EventArgs e)
        {
            if (_isDrawerOpen)
                await ShowDrawerAsync(false);
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            if (sender is not Button btn) return;

            var route = btn.CommandParameter as string;
            await ShowDrawerAsync(false);

            if (!string.IsNullOrWhiteSpace(route))
            {
                await Shell.Current.GoToAsync(route);
            }
        }

        private async Task ShowDrawerAsync(bool show)
        {
            if (show)
            {
                Backdrop.InputTransparent = false;
                await Task.WhenAll(
                    Backdrop.FadeTo(0.5, 200, Easing.CubicInOut),
                    Drawer.TranslateTo(0, 0, 200, Easing.CubicInOut)
                );
                _isDrawerOpen = true;
            }
            else
            {
                await Task.WhenAll(
                    Backdrop.FadeTo(0, 200, Easing.CubicInOut),
                    Drawer.TranslateTo(-1000, 0, 200, Easing.CubicInOut)
                );
                Backdrop.InputTransparent = true;
                _isDrawerOpen = false;
            }
        }
    }
}
