using SyndicApp.Mobile.ViewModels.Dashboard;

namespace SyndicApp.Mobile.Views.Dashboard;

public partial class SyndicDashboardPage : ContentPage
{
    public SyndicDashboardPage(SyndicDashboardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
