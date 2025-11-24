using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Lots;

namespace SyndicApp.Mobile.Views.Lots
{
    public partial class LotDetailsPage : ContentPage
    {
        public LotDetailsPage() : this(ServiceHelper.Services.GetRequiredService<LotDetailsViewModel>())
        {
        }

        public LotDetailsPage(LotDetailsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

            // Charge le lot au premier affichage
            Loaded += async (_, __) => await vm.LoadAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Gérer les droits (Modifier/Supprimer visibles uniquement pour le syndic)
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var roleLower = role.ToLowerInvariant();
                bool isSyndic = roleLower.Contains("syndic");

                BtnEdit.IsVisible = isSyndic;
                BtnDelete.IsVisible = isSyndic;

                // Le bouton retour reste visible pour tout le monde
                BtnBack.IsVisible = true;
            }
            catch
            {
                // En cas de souci, on laisse tout visible
                BtnEdit.IsVisible = true;
                BtnDelete.IsVisible = true;
                BtnBack.IsVisible = true;
            }
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
