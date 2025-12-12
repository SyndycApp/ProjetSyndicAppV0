using SyndicApp.Mobile.ViewModels.Communication;

namespace SyndicApp.Mobile.Views.Communication;

public partial class ConversationsPage : ContentPage
{
    ConversationsListViewModel Vm => BindingContext as ConversationsListViewModel;

    public ConversationsPage(ConversationsListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Vm.LoadConversationsAsync(); 
    }

   private async void OnConversationSelected(object sender, SelectionChangedEventArgs e)
{
    if (e.CurrentSelection.FirstOrDefault() is not ConversationItemViewModel item)
        return;

    ((CollectionView)sender).SelectedItem = null;

    await Shell.Current.GoToAsync(
        $"chat?conversationId={item.Id}&name={Uri.EscapeDataString(item.DisplayName)}"
    );
}

}
