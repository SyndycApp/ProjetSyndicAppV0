using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Dashboard;

namespace SyndicApp.Mobile.Views.Dashboard
{
    public partial class AffectationMaintenanceDashboardPage : ContentPage
    {
        public AffectationMaintenanceDashboardPage(AffectationMaintenanceDashboardViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AffectationMaintenanceDashboardViewModel vm)
                await vm.Load();
        }
    }
}
