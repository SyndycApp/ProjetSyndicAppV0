using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class IncidentStatusPage : ContentPage
    {
        private readonly IAccountApi _accountApi;

        public IncidentStatusPage(IncidentStatusViewModel vm, IAccountApi accountApi)
        {
            InitializeComponent();
            BindingContext = vm;
            _accountApi = accountApi;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is IncidentStatusViewModel vm)
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

                if (!isSyndic && !isGardien)
                {
                    await DisplayAlert("Accès refusé",
                        "Vous n’êtes pas autorisé à modifier le statut de cet incident.",
                        "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur rôles IncidentStatusPage : {ex}");
            }
        }
    }
}
