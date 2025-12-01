using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class AppelsPage : ContentPage
{
    public AppelsPage() : this(ServiceHelper.GetRequiredService<AppelsListViewModel>()) { }

    public AppelsPage(AppelsListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AppelsListViewModel vm && !vm.IsBusy)
            _ = vm.LoadAsync();
    }
}
