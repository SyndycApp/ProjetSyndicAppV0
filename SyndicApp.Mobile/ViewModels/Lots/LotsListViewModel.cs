using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Lots
{
    public partial class LotsListViewModel : ObservableObject, IRecipient<LotChangedMessage>
    {
        private readonly ILotsApi _api;

        [ObservableProperty] bool isBusy;
        [ObservableProperty] List<LotDto> items = new();

        public IAsyncRelayCommand LoadAsyncCommand { get; }
        public IAsyncRelayCommand OpenCreateAsyncCommand { get; }
        public IAsyncRelayCommand<Guid> OpenDetailsAsyncCommand { get; }

        public LotsListViewModel(ILotsApi api)
        {
            _api = api;
            LoadAsyncCommand = new AsyncRelayCommand(LoadAsync);
            OpenCreateAsyncCommand = new AsyncRelayCommand(OpenCreateAsync);
            OpenDetailsAsyncCommand = new AsyncRelayCommand<Guid>(OpenDetailsAsync);

            WeakReferenceMessenger.Default.Register<LotChangedMessage>(this, async (_, __) => await LoadAsync());
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;
            try { IsBusy = true; Items = (await _api.GetAllAsync()).ToList(); }
            finally { IsBusy = false; }
        }

        private Task OpenCreateAsync() => Shell.Current.GoToAsync("lot-create");
        private Task OpenDetailsAsync(Guid id) => Shell.Current.GoToAsync($"lot-details?id={id:D}");

        public async void Receive(LotChangedMessage message) => await LoadAsync();
    }
}
