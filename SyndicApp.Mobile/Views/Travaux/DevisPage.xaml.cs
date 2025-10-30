using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Travaux;

namespace SyndicApp.Mobile.Views.Travaux
{
    public partial class DevisPage : ContentPage
    {
        public DevisPage()
        {
            InitializeComponent();
        }

        private void OnAppearing(object? sender, EventArgs e)
        {
            if (BindingContext is DevisListViewModel vm)
            {
                _ = vm.LoadAsync();
            }
        }
    }
}
