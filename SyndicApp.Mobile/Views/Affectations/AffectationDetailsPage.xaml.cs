using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Affectations;
using SyndicApp.Mobile.Views.Layout;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationDetailsPage : RoleDrawerLayout
    {
        public AffectationDetailsPage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<AffectationDetailsViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is AffectationDetailsViewModel vm)
                await vm.LoadAsync();
        }
    }
}
