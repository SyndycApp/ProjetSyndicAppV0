using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Affectations;
using SyndicApp.Mobile.Views.Layout;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationHistoriquePage : RoleDrawerLayout
    {
        public AffectationHistoriquePage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<AffectationHistoriqueViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is AffectationHistoriqueViewModel vm)
                await vm.LoadAsync();
        }
    }
}
