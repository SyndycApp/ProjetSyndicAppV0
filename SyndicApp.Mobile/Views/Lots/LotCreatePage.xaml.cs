using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Lots;

namespace SyndicApp.Mobile.Views.Lots
{
    public partial class LotCreatePage : ContentPage
    {
        public LotCreatePage() : this(ServiceHelper.Services.GetRequiredService<LotCreateViewModel>())
        {
        }

        public LotCreatePage(LotCreateViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 🔐 Sécurité : seul le syndic peut créer un lot
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var roleLower = role.ToLowerInvariant();
                bool isSyndic = roleLower.Contains("syndic");

                if (!isSyndic)
                {
                    await DisplayAlert("Accès refusé", "Seul le syndic peut créer un lot.", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }
            }
            catch
            {
                // si problème de rôle, on laisse mais c'est très rare
            }

            // Charger résidences + bâtiments
            if (BindingContext is LotCreateViewModel vm)
                await vm.LoadAsync();
        }

        // Changement de résidence => recharger les bâtiments
        private async void ResidenceChanged(object sender, EventArgs e)
        {
            if (BindingContext is LotCreateViewModel vm)
                await vm.ResidenceChangedAsync();
        }
    }
}
