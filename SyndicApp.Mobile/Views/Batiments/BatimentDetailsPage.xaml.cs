using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Batiments;

namespace SyndicApp.Mobile.Views.Batiments
{
    public partial class BatimentDetailsPage : ContentPage
    {
        public BatimentDetailsPage(BatimentDetailsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            Loaded += async (_, __) => await vm.LoadAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var isSyndic = role.ToLowerInvariant().Contains("syndic");

                BtnEdit.IsVisible = isSyndic;
                BtnDelete.IsVisible = isSyndic;
            }
            catch
            {
                BtnEdit.IsVisible = true;
                BtnDelete.IsVisible = true;
            }
        }

        private async void Back_Clicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
