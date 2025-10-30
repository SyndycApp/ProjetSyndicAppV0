using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Lots;

namespace SyndicApp.Mobile.Views.Lots
{
    public partial class LotsPage : ContentPage
    {
        public LotsPage()
        {
            InitializeComponent();
        }

        private void OnAppearing(object? sender, EventArgs e)
        {
            if (BindingContext is LotsListViewModel vm)
            {
                _ = vm.LoadAsync();
            }
        }
    }
}
