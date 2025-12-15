using System.Collections.Specialized;
using SyndicApp.Mobile.ViewModels.Communication;

namespace SyndicApp.Mobile.Views.Communication;

public partial class ChatPage : ContentPage
{
    private ChatViewModel ViewModel => BindingContext as ChatViewModel;

    public ChatPage(ChatViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        // 🔽 AUTO-SCROLL quand un message est ajouté
        vm.Messages.CollectionChanged += Messages_CollectionChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await ViewModel.LoadMessagesCommand.ExecuteAsync(null);

            // Laisse le XAML créer les views
            await Task.Delay(50);

            ScrollToBottom();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur OnAppearing ChatPage : {ex}");
        }
    }

    // 🔔 Nouveau message → scroll automatique
    private void Messages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(100); // laisse le layout finir
            ScrollToBottom();
        });
    }

    private void ScrollToBottom()
    {
        try
        {
            if (MessagesStack == null || MessagesStack.Children.Count == 0)
                return;

            var last = MessagesStack.Children.Last() as View;
            if (last == null)
                return;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var y = last.Y;
                await MessagesScrollView.ScrollToAsync(0, y, true);
            });
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
