using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Residences;

namespace SyndicApp.Mobile.Views.Residences
{
    public partial class ResidencesPage : ContentPage
    {
        public ResidencesPage(ResidencesListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Gérer la visibilité du bouton "+" selon le rôle
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var roleLower = role.ToLowerInvariant();
                bool isSyndic = roleLower.Contains("syndic");

                BtnAddResidence.IsVisible = isSyndic;
            }
            catch
            {
                // En cas de souci, on laisse visible
                BtnAddResidence.IsVisible = true;
            }
        }

        // 🔹 CLICK SUR LE BOUTON "+"
        private async void OnAddResidenceClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("residence-create");
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erreur navigation",
                    $"Impossible d’ouvrir la page de création.\n\n{ex.Message}",
                    "OK");
            }
        }
    }
}
