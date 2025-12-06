using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Residences;
using System;

namespace SyndicApp.Mobile.Views.Residences
{
    public partial class ResidenceEditPage : ContentPage
    {
        private readonly ResidenceEditViewModel _vm;

        public ResidenceEditPage(ResidenceEditViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;

            Loaded += async (_, __) => await vm.LoadAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var role = Preferences.Get("user_role", string.Empty)?.ToLowerInvariant();
            if (role != "syndic")
            {
                Shell.Current.DisplayAlert("Accès refusé",
                    "Seul le Syndic peut modifier une résidence.",
                    "OK");

                Shell.Current.GoToAsync("..");
                return;
            }
        }

        private async void Back_Clicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
