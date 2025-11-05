using SyndicApp.Mobile.ViewModels.Residences;

namespace SyndicApp.Mobile.Views.Residences;

public partial class AddResidencePage : ContentPage
{
    public AddResidencePage(AddResidenceViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
