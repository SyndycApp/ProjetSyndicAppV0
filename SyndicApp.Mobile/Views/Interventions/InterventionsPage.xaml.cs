using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Interventions;

namespace SyndicApp.Mobile.Views.Interventions
{
    public partial class InterventionsPage : ContentPage
    {
        public InterventionsPage()
        {
            InitializeComponent();
        }

        private void OnAppearing(object? sender, EventArgs e)
        {
            if (BindingContext is InterventionsListViewModel vm)
            {
                _ = vm.LoadAsync();
            }
        }
    }
}
