using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Affectations;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationHistoriquePage : ContentPage
    {
        public AffectationHistoriquePage(AffectationHistoriqueViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AffectationHistoriqueViewModel vm)
                await vm.LoadAsync();
        }
    }
}
