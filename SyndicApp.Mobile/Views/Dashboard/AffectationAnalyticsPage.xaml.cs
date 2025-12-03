using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Dashboard;

namespace SyndicApp.Mobile.Views.Dashboard
{
    public partial class AffectationAnalyticsPage : ContentPage
    {
        public AffectationAnalyticsPage(AffectationAnalyticsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AffectationAnalyticsViewModel vm)
                await vm.Load();
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
