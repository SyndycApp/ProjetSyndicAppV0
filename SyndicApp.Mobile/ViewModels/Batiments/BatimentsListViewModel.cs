using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Batiments;

public partial class BatimentsListViewModel : ObservableObject, IRecipient<BatimentChangedMessage>
{
    private readonly IBatimentsApi _api;

    [ObservableProperty] bool isBusy;
    [ObservableProperty] List<BatimentDto> items = new();

    public IAsyncRelayCommand LoadAsyncCommand { get; }
    public IAsyncRelayCommand OpenCreateAsyncCommand { get; }
    public IAsyncRelayCommand<Guid> OpenDetailsAsyncCommand { get; }

    public BatimentsListViewModel(IBatimentsApi api)
    {
        _api = api;
        LoadAsyncCommand = new AsyncRelayCommand(LoadAsync);
        OpenCreateAsyncCommand = new AsyncRelayCommand(OpenCreateAsync);
        OpenDetailsAsyncCommand = new AsyncRelayCommand<Guid>(OpenDetailsAsync);

        WeakReferenceMessenger.Default.Register<BatimentChangedMessage>(this, async (_, __) => await LoadAsync());
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try { IsBusy = true; Items = (await _api.GetAllAsync()).ToList(); }
        finally { IsBusy = false; }
    }

    private Task OpenCreateAsync() => Shell.Current.GoToAsync("batiment-create");
    private Task OpenDetailsAsync(Guid id) => Shell.Current.GoToAsync($"batiment-details?id={id:D}");

    public async void Receive(BatimentChangedMessage message) => await LoadAsync();
}
