using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class DevisTravauxDetailsPage : ContentPage
    {
        private readonly DevisTravauxDetailsViewModel _viewModel;

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
    }
}
