using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class DevisTravauxDecisionPage : ContentPage
    {
        private readonly DevisTravauxDecisionViewModel _viewModel;

        public DevisTravauxDecisionPage(DevisTravauxDecisionViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await _viewModel.CancelAsync();
            }
            catch
            {
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
