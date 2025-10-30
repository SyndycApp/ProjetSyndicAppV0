using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances
{
    public partial class PaiementsPage : ContentPage
    {
        public PaiementsPage()
        {
            InitializeComponent();
        }

        private void OnAppearing(object? sender, EventArgs e)
        {
            if (BindingContext is PaiementsListViewModel vm)
            {
                _ = vm.LoadAsync();
            }
        }
    }
}
