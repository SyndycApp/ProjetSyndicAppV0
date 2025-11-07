using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;
public partial class AppelDetailsPage : ContentPage
{
    public AppelDetailsPage(AppelDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        Loaded += async (_, __) => await vm.LoadAsync();
    }
}
