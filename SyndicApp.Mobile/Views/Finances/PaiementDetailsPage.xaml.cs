using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class PaiementDetailsPage : ContentPage
{
    public PaiementDetailsPage() : this(ServiceHelper.GetRequiredService<PaiementDetailsViewModel>()) { }

    public PaiementDetailsPage(PaiementDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PaiementDetailsViewModel vm)
        {
            _ = vm.LoadAsync();
        }
    }
}
