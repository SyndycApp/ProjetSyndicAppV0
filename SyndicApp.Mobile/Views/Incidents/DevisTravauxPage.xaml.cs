using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class DevisTravauxPage : ContentPage
    {
        private readonly DevisTravauxListViewModel _viewModel;

        public DevisTravauxPage(DevisTravauxListViewModel viewModel)
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
    }
}
