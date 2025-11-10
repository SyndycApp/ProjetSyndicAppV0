using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Affectations;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationDetailsPage : ContentPage
    {
        public AffectationDetailsPage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<AffectationDetailsViewModel>();
        }
    }
}
