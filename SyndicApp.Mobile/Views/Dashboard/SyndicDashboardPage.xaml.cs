using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Dashboard;

namespace SyndicApp.Mobile.Views.Dashboard
{
    public partial class SyndicDashboardPage : ContentPage
    {
        public SyndicDashboardPage(SyndicDashboardViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is SyndicDashboardViewModel vm)
                await vm.LoadKpisAsyncCommand.ExecuteAsync(null);
        }
    }
}
