using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class IncidentCreatePage : ContentPage
    {
        private readonly IAccountApi _accountApi;

        public IncidentCreatePage(IncidentCreateViewModel vm, IAccountApi accountApi)
        {
            InitializeComponent();
            BindingContext = vm;
            _accountApi = accountApi;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is IncidentCreateViewModel vm)
            {
                await vm.LoadAsync();
            }

            await ApplyRoleRestrictionsAsync();
        }

        private async Task ApplyRoleRestrictionsAsync()
        {
            try
            {
                var me = await _accountApi.MeAsync();
                var roles = me.Roles ?? new System.Collections.Generic.List<string>();

                bool isSyndic = roles.Any(r => r.Equals("Syndic", StringComparison.OrdinalIgnoreCase));
                bool isGardien = roles.Any(r => r.Equals("Gardien", StringComparison.OrdinalIgnoreCase));
                bool isCopro = roles.Any(r =>
                    r.Equals("Copropriétaire", StringComparison.OrdinalIgnoreCase) ||
                    r.Equals("Coproprietaire", StringComparison.OrdinalIgnoreCase) ||
                    r.Equals("Copro", StringComparison.OrdinalIgnoreCase));

                // Exemple : ici tout le monde peut créer un incident
                // Si tu veux bloquer certains rôles, fais-le ici.
                if (!isSyndic && !isGardien && !isCopro)
                {
                    await DisplayAlert("Accès refusé",
                        "Vous n’êtes pas autorisé à créer un incident.",
                        "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur rôles IncidentCreatePage : {ex}");
            }
        }
    }
}
