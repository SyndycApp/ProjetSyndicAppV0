using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class PaiementCreatePage : ContentPage
{
    public PaiementCreatePage() : this(ServiceHelper.GetRequiredService<PaiementCreateViewModel>()) { }

    public PaiementCreatePage(PaiementCreateViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PaiementCreateViewModel vm)
        {
            _ = vm.LoadAsync();
        }
    }
}
