using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Affectations;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationCreatePage : ContentPage
    {
        public AffectationCreatePage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<AffectationCreateViewModel>();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is AffectationCreateViewModel vm)
                await vm.LoadAsync();
        }
    }
}
