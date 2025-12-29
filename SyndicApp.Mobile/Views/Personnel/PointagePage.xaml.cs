using SyndicApp.Mobile.ViewModels.Personnel;

namespace SyndicApp.Mobile.Views.Personnel;

public partial class PresencePage : ContentPage
{
    private readonly PresenceViewModel _vm;

    public PresencePage(PresenceViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }
}
