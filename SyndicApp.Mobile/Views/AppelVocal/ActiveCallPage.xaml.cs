namespace SyndicApp.Mobile.Views.AppelVocal;

public partial class ActiveCallPage : ContentPage
{
    public ActiveCallPage()
    {
        InitializeComponent();
        BindingContext = ServiceHelper.Services.GetRequiredService<ActiveCallViewModel>();
    }
}
