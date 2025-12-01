using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class IncidentDetailsPage : ContentPage
    {
        private readonly IAccountApi _accountApi;

        public IncidentDetailsPage(IncidentDetailsViewModel vm, IAccountApi accountApi)
        {
            InitializeComponent();
            BindingContext = vm;
            _accountApi = accountApi;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is IncidentDetailsViewModel vm)
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

                // Copro peut voir les détails, mais pas modifier / supprimer
                bool canModify = isSyndic || isGardien;

                BtnChangeStatus.IsVisible = canModify;
                BtnEdit.IsVisible = canModify;
                BtnDelete.IsVisible = canModify;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur rôles IncidentDetailsPage : {ex}");
                // En cas d’erreur → on laisse tout visible
            }
        }
    }
}
