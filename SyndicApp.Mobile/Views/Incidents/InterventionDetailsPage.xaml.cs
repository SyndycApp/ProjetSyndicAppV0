using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class InterventionDetailsPage : ContentPage
    {
        public InterventionDetailsPage(InterventionDetailsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is InterventionDetailsViewModel vm)
                await vm.LoadAsync();
        }
    }
}
