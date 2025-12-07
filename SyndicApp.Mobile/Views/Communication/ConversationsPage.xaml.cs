using SyndicApp.Mobile.ViewModels.Communication;

namespace SyndicApp.Mobile.Views.Communication;

public partial class ConversationsPage : ContentPage
{
    private ConversationsListViewModel ViewModel => BindingContext as ConversationsListViewModel;

    public ConversationsPage(ConversationsListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.LoadConversationsCommand.ExecuteAsync(null);
    }
}
