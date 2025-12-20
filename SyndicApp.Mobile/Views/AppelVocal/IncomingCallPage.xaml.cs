

namespace SyndicApp.Mobile.Views.AppelVocal;

public partial class IncomingCallPage : ContentPage
{
    public IncomingCallPage(IncomingCallViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
