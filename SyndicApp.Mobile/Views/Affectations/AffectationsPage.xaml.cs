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

            // Gestion rôle : seul le syndic voit le bouton "+"
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var roleLower = role.ToLowerInvariant();
                bool isSyndic = roleLower.Contains("syndic");

                BtnAddAffectation.IsVisible = isSyndic;
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
                    await DisplayAlert("Erreur",
                        $"Impossible de charger les affectations.\n\n{ex.Message}",
                        "OK");
                }
            }
        }

        // + Nouvelle affectation
        private async void OnAddAffectationClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("affectation-create");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur navigation",
                    $"Impossible d’ouvrir la page de création.\n\n{ex.Message}",
                    "OK");
            }
        }

        // Détails : on récupère juste une propriété Id (Guid) sur l’objet de la ligne
        private async void OnDetailsClicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.BindingContext is object item)
                {
                    var idProp = item.GetType().GetProperty("Id");
                    if (idProp?.GetValue(item) is Guid id)
                    {
                        await Shell.Current.GoToAsync($"affectation-details?id={id:D}");
                    }
                    else
                    {
                        await DisplayAlert("Erreur",
                            "Impossible de récupérer l'identifiant de l'affectation.",
                            "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur navigation",
                    $"Impossible d’ouvrir le détail.\n\n{ex.Message}",
                    "OK");
            }
        }
    }
}
