using SyndicApp.Mobile.ViewModels.Communication;

namespace SyndicApp.Mobile.Views.Communication;

public partial class ChatPage : ContentPage
{
    private ChatViewModel ViewModel => BindingContext as ChatViewModel;

    public ChatPage(ChatViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        this.Dispatcher.Dispatch(() =>
    {
        MessagesList.ItemsSource = vm.Messages;
    });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Charge les messages
            await ViewModel.LoadMessagesCommand.ExecuteAsync(null);

            // Scroll auto
            ScrollToBottom();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur OnAppearing ChatPage : {ex}");
        }
    }

    /// <summary>
    /// Scroll automatiquement vers le dernier message.
    /// </summary>
    private void ScrollToBottom()
    {
        try
        {
            var items = MessagesList?.ItemsSource?.Cast<object>().ToList();

            if (items != null && items.Any())
            {
                var last = items.Last();
                MessagesList.ScrollTo(last, position: ScrollToPosition.End, animate: true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur ScrollToBottom() : {ex}");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur retour ChatPage : {ex}");
        }
    }
}
