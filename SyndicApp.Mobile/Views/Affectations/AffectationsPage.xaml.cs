using System;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.ViewModels.Affectations;
using SyndicApp.Mobile.Views.Layout;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationsPage : RoleDrawerLayout
    {
        public AffectationsPage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<AffectationsListViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is AffectationsListViewModel vm)
                await vm.LoadAsync();
        }

        private async void OnDetailsClicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is not Button btn)
                    return;

                if (btn.BindingContext is not AffectationLotDto item)
                    return;

                await Shell.Current.GoToAsync($"affectation-details?id={item.Id}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }
    }
}
