namespace SyndicApp.Mobile.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;


public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? error;
    public bool IsNotBusy => !IsBusy;
}