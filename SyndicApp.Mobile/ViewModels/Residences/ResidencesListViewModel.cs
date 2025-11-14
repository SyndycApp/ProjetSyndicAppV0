using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Residences;

public partial class ResidencesListViewModel : ObservableObject, IRecipient<ResidenceChangedMessage>
{
    private readonly IResidencesApi _api;

    [ObservableProperty] private bool isBusy;

    [ObservableProperty] private List<ResidenceDto> residences = new();


    public IAsyncRelayCommand LoadAsyncCommand { get; }
    public IAsyncRelayCommand OpenCreateAsyncCommand { get; }                
    public IAsyncRelayCommand<Guid> OpenDetailsAsyncCommand { get; }       

    public ResidencesListViewModel(IResidencesApi api)
    {
        _api = api;

        LoadAsyncCommand = new AsyncRelayCommand(LoadAsync);
        OpenCreateAsyncCommand = new AsyncRelayCommand(OpenCreateAsync);
        OpenDetailsAsyncCommand = new AsyncRelayCommand<Guid>(OpenDetailsAsync);

        WeakReferenceMessenger.Default.Register<BatimentChangedMessage>(this,
              async (_, __) => await LoadAsync());
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var list = await _api.GetAllAsync();
            Residences = list.ToList();
        }
        finally { IsBusy = false; }
    }

    private Task OpenCreateAsync()
        => Shell.Current.GoToAsync("residence-create");

    private Task OpenDetailsAsync(Guid id)
        => Shell.Current.GoToAsync($"residence-details?id={id:D}");


    public async void Receive(ResidenceChangedMessage message) => await LoadAsync();
}
