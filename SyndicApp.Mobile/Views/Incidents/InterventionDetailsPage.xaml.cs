using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class InterventionDetailsPage : ContentPage
    {
        public InterventionDetailsPage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<InterventionDetailsViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is InterventionDetailsViewModel vm)
                await vm.LoadAsync();
        }
    }
}
