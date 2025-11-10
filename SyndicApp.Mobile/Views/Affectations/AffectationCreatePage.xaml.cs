using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Affectations;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationCreatePage : ContentPage
    {
        public AffectationCreatePage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<AffectationCreateViewModel>();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is AffectationCreateViewModel vm)
                await vm.LoadAsync();
        }

        private async void OnSaveClicked(object? sender, EventArgs e)
        {
            
            if (BindingContext is AffectationCreateViewModel vm)
            {
                try
                {
                    await vm.CreateAsync(); // appelle la méthode marquée [RelayCommand]
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erreur", ex.Message, "OK");
                }
            }
        }
    }
}
