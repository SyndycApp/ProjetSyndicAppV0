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

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
