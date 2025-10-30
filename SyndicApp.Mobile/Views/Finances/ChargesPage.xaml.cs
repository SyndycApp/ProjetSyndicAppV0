using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances
{
    public partial class ChargesPage : ContentPage
    {
        public ChargesPage()
        {
            InitializeComponent();
        }

        private void OnAppearing(object? sender, EventArgs e)
        {
            if (BindingContext is ChargesListViewModel vm)
            {
                _ = vm.LoadAsync();
            }
        }
    }
}
