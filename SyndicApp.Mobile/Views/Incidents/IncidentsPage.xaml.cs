using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class IncidentsPage : ContentPage
    {
        public IncidentsPage()
        {
            InitializeComponent();
        }

        private void OnAppearing(object? sender, EventArgs e)
        {
            if (BindingContext is IncidentsListViewModel vm)
            {
                _ = vm.LoadAsync();
            }
        }
    }
}
