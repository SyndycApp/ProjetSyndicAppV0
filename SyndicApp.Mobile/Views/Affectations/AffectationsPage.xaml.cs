using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Affectations;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationsPage : ContentPage
    {
        public AffectationsPage(AffectationsListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Rôle : seul le syndic peut créer
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                BtnAddAffectation.IsVisible = role.ToLower().Contains("syndic");
            }
            catch
            {
                BtnAddAffectation.IsVisible = true;
            }

            if (BindingContext is AffectationsListViewModel vm)
            {
                try
                {
                    await vm.LoadAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erreur", $"Impossible de charger : {ex.Message}", "OK");
                }
            }
        }

        private async void OnAddAffectationClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("affectation-create");
        }

        private async void OnDetailsClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.BindingContext is object item)
            {
                var idProp = item.GetType().GetProperty("Id");
                if (idProp?.GetValue(item) is Guid id)
                    await Shell.Current.GoToAsync($"affectation-details?id={id}");
            }
        }
    }
}
