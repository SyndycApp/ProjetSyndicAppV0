using SyndicApp.Mobile.ViewModels.Auth;

namespace SyndicApp.Mobile.Views.Auth;

public partial class ResetWithCodePage : ContentPage
{
    public ResetWithCodePage(ResetWithCodeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
