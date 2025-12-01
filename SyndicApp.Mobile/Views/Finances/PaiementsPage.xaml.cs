using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class PaiementsPage : ContentPage
{
    public PaiementsPage() : this(ServiceHelper.GetRequiredService<PaiementsListViewModel>()) { }

    public PaiementsPage(PaiementsListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PaiementsListViewModel vm)
        {
            _ = vm.LoadAsync();
        }
    }
}
