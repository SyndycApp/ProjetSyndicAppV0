using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Dashboard;

namespace SyndicApp.Mobile.Views.Dashboard
{
    public partial class AffectationUserDashboardPage : ContentPage
    {
        public AffectationUserDashboardPage(AffectationUserDashboardViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AffectationUserDashboardViewModel vm)
                await vm.Load();
        }
    }
}
