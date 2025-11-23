using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Batiments;

namespace SyndicApp.Mobile.Views.Batiments
{
    public partial class BatimentEditPage : ContentPage
    {
        public BatimentEditPage(BatimentEditViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Vérifier rôle : seul syndic peut modifier
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var isSyndic = role.ToLowerInvariant().Contains("syndic");

                if (!isSyndic)
                {
                    await DisplayAlert("Accès refusé",
                        "Seul le syndic peut modifier un bâtiment.",
                        "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }
            }
            catch
            {
                // si problème de rôle, on laisse passer
            }

            if (BindingContext is BatimentEditViewModel vm)
                await vm.LoadAsync();
        }

        private async void Back_Clicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
