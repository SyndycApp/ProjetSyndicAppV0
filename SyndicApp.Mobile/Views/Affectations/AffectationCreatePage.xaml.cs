using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Affectations;
using SyndicApp.Mobile.Views.Layout;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationCreatePage : RoleDrawerLayout
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
