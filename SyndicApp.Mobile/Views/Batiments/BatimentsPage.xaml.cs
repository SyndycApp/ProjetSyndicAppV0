using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.ViewModels.Batiments;

namespace SyndicApp.Mobile.Views.Batiments
{
    public partial class BatimentsPage : ContentPage
    {
        public BatimentsPage(BatimentsListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 1) Charger la liste
            if (BindingContext is BatimentsListViewModel vm)
                await vm.LoadAsync();

            // 2) Gérer la visibilité du bouton +
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var isSyndic = role.ToLowerInvariant().Contains("syndic");
                BtnAddBatiment.IsVisible = isSyndic;
            }
            catch
            {
                BtnAddBatiment.IsVisible = true;
            }
        }

        // 👉 clic sur +
        private async void OnAddBatimentClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("batiment-create");
        }

        // 👉 clic sur une carte de bâtiment
        private async void OnBatimentTapped(object sender, TappedEventArgs e)
        {
            try
            {
                if (sender is Frame frame && frame.BindingContext is BatimentDto dto)
                {
                    // Navigation vers la page de détails
                    await Shell.Current.GoToAsync($"batiment-details?id={dto.Id:D}");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Impossible d'ouvrir le détail du bâtiment.\n\n{ex.Message}", "OK");
            }
        }
    }
}
