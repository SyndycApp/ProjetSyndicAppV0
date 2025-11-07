using SyndicApp.Mobile.ViewModels.Dashboard;
using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.Views.Dashboard;

public partial class SyndicDashboardPage : ContentPage
{
    // ⬇️ ctor par défaut pour Shell/DataTemplate
    public SyndicDashboardPage() : this(ServiceHelper.GetRequiredService<SyndicDashboardViewModel>()) { }

    public SyndicDashboardPage(SyndicDashboardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
