namespace SyndicApp.Mobile.ViewModels.Common;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty] bool isBusy;
    [ObservableProperty] string? title;
}
