using SyndicApp.Mobile.Models;
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
        Console.WriteLine("=== 📌 ConversationsPage.OnAppearing ===");
        await Vm.LoadConversationsAsync();
        Console.WriteLine($"🟣 UI After Load → ConvList.Count = {Vm.Conversations.Count}");

        Console.WriteLine("UI Loaded → ConvList visible? " + (ConvList?.Height));

        Console.WriteLine("=== 📌 ConversationsPage END ===");
    }

    private async void OnConversationSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not ConversationDto item)
            return;

        // IMPORTANT : désélectionner pour permettre un second clic
        ((CollectionView)sender).SelectedItem = null;

        await Shell.Current.GoToAsync($"chat?conversationId={item.Id}");
    }
}
