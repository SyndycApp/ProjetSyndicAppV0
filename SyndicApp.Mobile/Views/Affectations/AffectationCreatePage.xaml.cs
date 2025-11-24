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
                await vm.LoadAsync();

            // restriction : seul le syndic peut enregistrer
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
}
