using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Lots
{
    public partial class LotCreateViewModel : ObservableObject
    {
        private readonly ILotsApi _lotsApi;
        private readonly IResidencesApi _resApi;
        private readonly IBatimentsApi _batApi;

        [ObservableProperty] string numeroLot = "";
        [ObservableProperty] string type = "";
        [ObservableProperty] double surface;

        [ObservableProperty] List<ResidenceDto> residences = new();
        [ObservableProperty] ResidenceDto? selectedResidence;

        [ObservableProperty] List<BatimentDto> batiments = new();
        [ObservableProperty] BatimentDto? selectedBatiment;

        [ObservableProperty] bool isBusy;

        public LotCreateViewModel(ILotsApi lotsApi, IResidencesApi resApi, IBatimentsApi batApi)
        {
            _lotsApi = lotsApi; _resApi = resApi; _batApi = batApi;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                Residences = await _resApi.GetAllAsync();
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        public async Task ResidenceChangedAsync()
        {
            Batiments = new();
            SelectedBatiment = null;

            if (SelectedResidence is null) return;

            // Pas d’API by-residence -> on filtre localement
            var all = await _batApi.GetAllAsync();
            Batiments = all.Where(b => b.ResidenceId == SelectedResidence.Id).ToList();
        }

        [RelayCommand]
        private async Task CreateAsync()
        {
            if (string.IsNullOrWhiteSpace(NumeroLot) || string.IsNullOrWhiteSpace(Type) || SelectedResidence is null)
            {
                await Shell.Current.DisplayAlert("Validation", "Numéro, Type et Résidence sont obligatoires.", "OK");
                return;
            }

            // Résolution Résidence (si besoin)
            var resId = SelectedResidence.Id;
            if (resId == Guid.Empty && !string.IsNullOrWhiteSpace(SelectedResidence.Nom))
            {
                var looked = await _resApi.LookupIdAsync(SelectedResidence.Nom); // retourne Guid (non-nullable)
                resId = looked;
            }

            // Résolution Bâtiment (si besoin)
            Guid? batId = SelectedBatiment != null ? SelectedBatiment.Id : (Guid?)null;
            if (batId is null && SelectedBatiment is not null && !string.IsNullOrWhiteSpace(SelectedBatiment.Nom))
            {
                batId = await _batApi.ResolveIdAsync(SelectedBatiment.Nom); // Guid?
            }

            try
            {
                await _lotsApi.CreateAsync(new CreateLotDto
                {
                    NumeroLot = NumeroLot.Trim(),
                    Type = Type.Trim(),
                    Surface = Surface,
                    ResidenceId = resId,
                    BatimentId = batId
                });

                WeakReferenceMessenger.Default.Send(new LotChangedMessage(true));
                await Shell.Current.DisplayAlert("Succès", "Lot créé.", "OK");
                await Shell.Current.GoToAsync("//lots");
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Erreur API", ex.Content ?? ex.Message, "OK");
            }
        }

        [RelayCommand] private Task CancelAsync() => Shell.Current.GoToAsync("//lots");
    }
}

