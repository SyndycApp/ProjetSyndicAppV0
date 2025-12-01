using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class IncidentsPage : ContentPage
    {
        private readonly IAccountApi _accountApi;

        public IncidentsPage(IncidentsListViewModel vm, IAccountApi accountApi)
        {
            InitializeComponent();
            BindingContext = vm;
            _accountApi = accountApi;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is IncidentsListViewModel vm)
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

                // ici on pourrait, par ex, limiter complètement pour certains rôles
                // pour l’instant : tout le monde peut voir la liste → pas de blocage
                bool isSyndic = roles.Any(r => r.Equals("Syndic", StringComparison.OrdinalIgnoreCase));
                bool isGardien = roles.Any(r => r.Equals("Gardien", StringComparison.OrdinalIgnoreCase));
                bool isCopro = roles.Any(r =>
                    r.Equals("Copropriétaire", StringComparison.OrdinalIgnoreCase) ||
                    r.Equals("Coproprietaire", StringComparison.OrdinalIgnoreCase) ||
                    r.Equals("Copro", StringComparison.OrdinalIgnoreCase));

                if (!isSyndic && !isGardien && !isCopro)
                {
                    await DisplayAlert("Accès refusé",
                        "Vous n’êtes pas autorisé à consulter les incidents.",
                        "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur rôles IncidentsPage : {ex}");
                // En cas d’erreur → on laisse passer (à toi d’ajuster si tu veux bloquer)
            }
        }
    }
}
