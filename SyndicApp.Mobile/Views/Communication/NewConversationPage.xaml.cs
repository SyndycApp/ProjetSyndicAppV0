using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.ViewModels.Communication;

namespace SyndicApp.Mobile.Views.Communication;

public partial class NewConversationPage : ContentPage
{
    private NewConversationViewModel Vm => BindingContext as NewConversationViewModel;

    public NewConversationPage(NewConversationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Vm.LoadAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    // 👤 Sélection utilisateur
    private async void OnUserSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not ChatUserDto user)
            return;

        ((CollectionView)sender).SelectedItem = null;

        await Vm.OpenAsync(user);
    }
}
