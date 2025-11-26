using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Affectations;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationDetailsPage : ContentPage
    {
        public AffectationDetailsPage(AffectationDetailsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AffectationDetailsViewModel vm)
                await vm.LoadAsync();

            // seules les actions d'édition / clôture sont pour le syndic
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var isSyndic = role.ToLowerInvariant().Contains("syndic");
                BtnEdit.IsVisible = isSyndic;
                BtnClose.IsVisible = isSyndic;
            }
            catch
            {
                BtnEdit.IsVisible = true;
                BtnClose.IsVisible = true;
            }
        }

        // 🔹 SÉCURITÉ : on déclenche les commandes du VM depuis les Clicked

        private void OnEditClicked(object sender, EventArgs e)
        {
            if (BindingContext is AffectationDetailsViewModel vm &&
                vm.EditCommand != null &&
                vm.EditCommand.CanExecute(null))
            {
                vm.EditCommand.Execute(null);
            }
        }

        private void OnCloseClicked(object sender, EventArgs e)
        {
            if (BindingContext is AffectationDetailsViewModel vm &&
                vm.CloseCommand != null &&
                vm.CloseCommand.CanExecute(null))
            {
                vm.CloseCommand.Execute(null);
            }
        }

        private void OnHistoriqueClicked(object sender, EventArgs e)
        {
            if (BindingContext is AffectationDetailsViewModel vm &&
                vm.GoToHistoriqueCommand != null &&
                vm.GoToHistoriqueCommand.CanExecute(null))
            {
                vm.GoToHistoriqueCommand.Execute(null);
            }
        }
    }
}
