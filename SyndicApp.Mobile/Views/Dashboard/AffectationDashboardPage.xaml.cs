using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Dashboard;

namespace SyndicApp.Mobile.Views.Dashboard
{
    public partial class AffectationDashboardPage : ContentPage
    {
        public AffectationDashboardPage(AffectationDashboardViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AffectationDashboardViewModel vm)
                await vm.Load();
        }
    }
}
