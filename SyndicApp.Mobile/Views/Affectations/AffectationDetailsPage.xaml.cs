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

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AffectationDetailsViewModel vm)
                await vm.LoadAsync();

            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var isSyndic = role.ToLowerInvariant().Contains("syndic");

                BtnEdit.IsVisible  = isSyndic;
                BtnClose.IsVisible = isSyndic;
            }
            catch
            {
                BtnEdit.IsVisible  = true;
                BtnClose.IsVisible = true;
            }
        }
    }
}
