using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Personnel;

namespace SyndicApp.Mobile.Views.Personnel
{
    public partial class PrestataireDetailsPage : ContentPage
    {
        public PrestataireDetailsPage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<PrestataireDetailsViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PrestataireDetailsViewModel vm)
                await vm.LoadAsync();
        }
    }
}
