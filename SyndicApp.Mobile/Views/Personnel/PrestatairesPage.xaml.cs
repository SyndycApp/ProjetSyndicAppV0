using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Personnel;

namespace SyndicApp.Mobile.Views.Personnel
{
    public partial class PrestatairesPage : ContentPage
    {
        public PrestatairesPage(PrestatairesListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PrestatairesListViewModel vm)
                await vm.LoadAsync();
        }
    }
}
