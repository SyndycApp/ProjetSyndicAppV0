using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Lots;

namespace SyndicApp.Mobile.Views.Lots
{
    public partial class LotsPage : ContentPage
    {
        public LotsPage() : this(ServiceHelper.Services.GetRequiredService<LotsListViewModel>())
        {
        }

        public LotsPage(LotsListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 1) Charger les lots
            if (BindingContext is LotsListViewModel vm)
            {
                await vm.LoadAsync();
            }

            // 2) Gérer la visibilité du bouton "+" selon le rôle
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var roleLower = role.ToLowerInvariant();
                bool isSyndic = roleLower.Contains("syndic");

                BtnAddLot.IsVisible = isSyndic;
            }
            catch
            {
                // En cas de problème, on laisse visible
                BtnAddLot.IsVisible = true;
            }
        }

        // 🔹 Click sur le bouton "+"
        private async void OnAddLotClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("lot-create");
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erreur navigation",
                    $"Impossible d’ouvrir la page de création du lot.\n\n{ex.Message}",
                    "OK");
            }
        }
    }
}
