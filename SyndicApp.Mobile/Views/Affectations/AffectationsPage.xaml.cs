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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Gérer la visibilité du bouton "+" selon le rôle
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var roleLower = role.ToLowerInvariant();
                var isSyndic = roleLower.Contains("syndic");

                BtnAddAffectation.IsVisible = isSyndic;
            }
            catch
            {
                BtnAddAffectation.IsVisible = true;
            }
        }

        private async void OnDetailsClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Guid id)
            {
                await Shell.Current.GoToAsync($"affectation-details?id={id:D}");
            }
        }

        // si tu veux utiliser le bouton "+" pour ouvrir la création via code-behind
        private async void OnAddAffectationClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("affectation-create");
        }
    }
}
