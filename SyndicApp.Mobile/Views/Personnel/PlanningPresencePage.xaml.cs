using SyndicApp.Mobile.ViewModels.Personnel;

namespace SyndicApp.Mobile.Views.Personnel;

public partial class PlanningPresencePage : ContentPage
{
    public PlanningPresencePage(PlanningPresenceViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((PlanningPresenceViewModel)BindingContext).LoadAsync();
    }
}
