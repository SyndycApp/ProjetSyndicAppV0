using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Personnel;

namespace SyndicApp.Mobile.Views.Personnel
{
    public partial class PrestataireCreatePage : ContentPage
    {
        public PrestataireCreatePage()
        {
            InitializeComponent();
            BindingContext ??= ServiceHelper.Get<PrestataireCreateViewModel>();
        }
    }
}
