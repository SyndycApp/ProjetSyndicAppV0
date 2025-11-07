using SyndicApp.Mobile.ViewModels.Finances;
using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.Views.Finances;
public partial class AppelsPage : ContentPage
{

    public AppelsPage() : this(ServiceHelper.GetRequiredService<AppelsListViewModel>()) { }
    public AppelsPage(AppelsListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        Loaded += async (_, __) => await vm.LoadAsync();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("//syndic-dashboard");
}
