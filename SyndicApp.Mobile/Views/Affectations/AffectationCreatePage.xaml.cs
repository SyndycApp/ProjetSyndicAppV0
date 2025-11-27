// SyndicApp.Mobile/Views/Affectations/AffectationCreatePage.xaml.cs
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Affectations;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationCreatePage : ContentPage
    {
        public AffectationCreatePage(AffectationCreateViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AffectationCreateViewModel vm)
            {
                // charge les users + lots
                await vm.LoadAsync();

                // seul le syndic peut créer / modifier
                try
                {
                    var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                    var isSyndic = role.ToLowerInvariant().Contains("syndic");
                    BtnSave.IsVisible = isSyndic;
                }
                catch
                {
                    BtnSave.IsVisible = true;
                }
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (BindingContext is AffectationCreateViewModel vm)
            {
                // CreateAsync gère création OU édition (IsEdit)
                await vm.CreateAsync();
            }
        }
    }
}
