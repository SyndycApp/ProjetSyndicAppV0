using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.LocatairesTemp;

namespace SyndicApp.Mobile.Views.LocatairesTemp
{
    public partial class LocatairesTempPage : ContentPage
    {
        public LocatairesTempPage()
        {
            InitializeComponent();
        }

        private void OnAppearing(object? sender, EventArgs e)
        {
            if (BindingContext is LocatairesTempListViewModel vm)
            {
                _ = vm.LoadAsync();
            }
        }
    }
}
