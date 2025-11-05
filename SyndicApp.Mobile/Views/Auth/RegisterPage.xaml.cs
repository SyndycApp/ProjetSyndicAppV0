using SyndicApp.Mobile.ViewModels.Auth;

namespace SyndicApp.Mobile.Views.Auth
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(RegisterViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
