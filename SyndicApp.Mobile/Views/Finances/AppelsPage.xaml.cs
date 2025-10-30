using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances
{
    public partial class AppelsPage : ContentPage
    {
        public AppelsPage()
        {
            InitializeComponent();
        }

        private void OnAppearing(object? sender, EventArgs e)
        {
            if (BindingContext is AppelsListViewModel vm)
            {
                _ = vm.LoadAsync();
            }
        }
    }
}
