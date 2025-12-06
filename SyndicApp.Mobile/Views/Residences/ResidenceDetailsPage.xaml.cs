using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Residences;

namespace SyndicApp.Mobile.Views.Residences
{
    public partial class ResidenceDetailsPage : ContentPage
    {
        private readonly ResidenceDetailsViewModel _vm;

        public ResidenceDetailsPage(ResidenceDetailsViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;

            Loaded += async (_, __) => await vm.LoadAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // masque le bouton delete si rôle != syndic
            var role = Preferences.Get("user_role", string.Empty)?.ToLowerInvariant();
            DeleteButton.IsVisible = role == "syndic";
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is ResidenceDetailsViewModel vm)
                await vm.DeleteAsync();
        }
    }
}
