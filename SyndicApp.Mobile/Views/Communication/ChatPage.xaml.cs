using SyndicApp.Mobile.ViewModels.Communication;

namespace SyndicApp.Mobile.Views.Communication;

public partial class ChatPage : ContentPage
{
    private ChatViewModel ViewModel => BindingContext as ChatViewModel;

    public ChatPage(ChatViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Charger les messages
        await ViewModel.LoadMessagesCommand.ExecuteAsync(null);

        // Scroll automatique vers le bas
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        if (MessagesList.ItemsSource is not null &&
            MessagesList.ItemsSource.Cast<object>().Any())
        {
            var last = MessagesList.ItemsSource.Cast<object>().Last();
            MessagesList.ScrollTo(last, position: ScrollToPosition.End, animate: true);
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(".."); // Retour à la liste des conversations
    }
}
