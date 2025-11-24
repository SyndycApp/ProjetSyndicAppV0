using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Lots;

namespace SyndicApp.Mobile.Views.Lots
{
    public partial class LotEditPage : ContentPage
    {
        public LotEditPage() : this(ServiceHelper.Services.GetRequiredService<LotEditViewModel>())
        {
        }

        public LotEditPage(LotEditViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

            // Charge les données du lot + listes au premier affichage
            Loaded += async (_, __) => await vm.LoadAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 🔐 Sécurité : seul le syndic peut modifier un lot
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var roleLower = role.ToLowerInvariant();
                bool isSyndic = roleLower.Contains("syndic");

                if (!isSyndic)
                {
                    await DisplayAlert("Accès refusé", "Seul le syndic peut modifier un lot.", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }
            }
            catch
            {
                // Si problème rôle, on laisse pour ne pas bloquer en dev
            }
        }

        private async void ResidenceChanged(object sender, EventArgs e)
        {
            if (BindingContext is LotEditViewModel vm)
                await vm.ResidenceChangedAsync();
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
