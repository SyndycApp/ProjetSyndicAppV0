using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Batiments;

namespace SyndicApp.Mobile.Views.Batiments
{
    public partial class BatimentsPage : ContentPage
    {
        public BatimentsPage()
        {
            InitializeComponent();
        }

        private void OnAppearing(object? sender, EventArgs e)
        {
            if (BindingContext is BatimentsListViewModel vm)
            {
                _ = vm.LoadAsync();
            }
        }
    }
}
