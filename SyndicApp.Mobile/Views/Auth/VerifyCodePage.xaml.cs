using SyndicApp.Mobile.ViewModels.Auth;

namespace SyndicApp.Mobile.Views.Auth;

public partial class VerifyCodePage : ContentPage
{
    public VerifyCodePage(VerifyCodeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
