using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class AppelDetailsPage : ContentPage
{
    public AppelDetailsPage(AppelDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
