using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Batiments;

namespace SyndicApp.Mobile.Views.Batiments
{
    public partial class BatimentCreatePage : ContentPage
    {
        public BatimentCreatePage(BatimentCreateViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Vérifier le rôle : seul le syndic peut créer
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var isSyndic = role.ToLowerInvariant().Contains("syndic");

                if (!isSyndic)
                {
                    await DisplayAlert("Accès refusé",
                        "Seul le syndic peut créer un bâtiment.",
                        "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }
            }
            catch
            {
                // si problème de rôle, on laisse passer
            }

            if (BindingContext is BatimentCreateViewModel vm)
                await vm.LoadResidencesAsync();
        }
    }
}
