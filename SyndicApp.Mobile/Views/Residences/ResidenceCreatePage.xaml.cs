using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Residences;

namespace SyndicApp.Mobile.Views.Residences
{
    public partial class ResidenceCreatePage : ContentPage
    {
        private readonly ResidenceCreateViewModel _vm;

        public ResidenceCreatePage(ResidenceCreateViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var role = Preferences.Get("user_role", string.Empty)?.ToLowerInvariant();
            if (role != "syndic")
            {
                Shell.Current.DisplayAlert("Accès refusé",
                    "Seul le Syndic peut créer une résidence.",
                    "OK");

                Shell.Current.GoToAsync("//residences");
                return;
            }
        }

        private async void Back_Clicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
