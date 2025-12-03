using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Incidents;

namespace SyndicApp.Mobile.Views.Incidents
{
    public partial class InterventionsPage : ContentPage
    {
        public InterventionsPage(InterventionsListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is InterventionsListViewModel vm)
                await vm.LoadAsync();
        }
    }
}
