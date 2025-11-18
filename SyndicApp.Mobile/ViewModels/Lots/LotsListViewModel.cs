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
        private readonly ILotsApi _lotsApi;
        private readonly IAffectationsLotsApi _affectationsApi;

        [ObservableProperty] bool isBusy;
        [ObservableProperty] List<LotDto> items = new();

        public IAsyncRelayCommand LoadAsyncCommand { get; }
        public IAsyncRelayCommand OpenCreateAsyncCommand { get; }
        public IAsyncRelayCommand<Guid> OpenDetailsAsyncCommand { get; }

        public LotsListViewModel(ILotsApi lotsApi, IAffectationsLotsApi affectationsApi)
        {
            _lotsApi = lotsApi;
            _affectationsApi = affectationsApi;

            LoadAsyncCommand = new AsyncRelayCommand(LoadAsync);
            OpenCreateAsyncCommand = new AsyncRelayCommand(OpenCreateAsync);
            OpenDetailsAsyncCommand = new AsyncRelayCommand<Guid>(OpenDetailsAsync);

            WeakReferenceMessenger.Default.Register<LotChangedMessage>(this, async (_, __) => await LoadAsync());
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                // 1) Récupère tous les lots
                var lots = await _lotsApi.GetAllAsync();

                // 2) Récupère toutes les affectations
                var affectations = await _affectationsApi.GetAllAsync();

                // 3) Enrichit chaque lot avec son statut d’occupation
                foreach (var lot in lots)
                {
                    var affectationActive = affectations
                        .FirstOrDefault(a => a.LotId == lot.Id && a.DateFin == null);

                    lot.EstOccupe = affectationActive != null;
                    lot.OccupantNom = affectationActive?.UserNom;
                }

                Items = lots.ToList();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task OpenCreateAsync() => Shell.Current.GoToAsync("lot-create");
        private Task OpenDetailsAsync(Guid id) => Shell.Current.GoToAsync($"lot-details?id={id:D}");

        public async void Receive(LotChangedMessage message) => await LoadAsync();
    }
}
