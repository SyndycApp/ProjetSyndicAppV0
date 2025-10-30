using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Residences;

namespace SyndicApp.Mobile.Views.Residences
{
    public partial class ResidenceDetailsPage : ContentPage
    {
        public ResidenceDetailsPage()
        {
            InitializeComponent();
        }

        private void OnAppearing(object? sender, EventArgs e)
        {
            if (BindingContext is ResidenceDetailsViewModel vm)
            {
                _ = vm.LoadAsync();
            }
        }
    }
}
