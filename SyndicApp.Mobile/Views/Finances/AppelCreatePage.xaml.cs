using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;
public partial class AppelCreatePage : ContentPage
{
    public AppelCreatePage(AppelCreateViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
