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
    [QueryProperty(nameof(Id), "id")]
    public partial class LotEditViewModel : ObservableObject
    {
        private readonly ILotsApi _lotsApi;
        private readonly IResidencesApi _resApi;
        private readonly IBatimentsApi _batApi;

        [ObservableProperty] string id = "";
        [ObservableProperty] string numeroLot = "";
        [ObservableProperty] string type = "";
        [ObservableProperty] double surface;

        [ObservableProperty] List<ResidenceDto> residences = new();
        [ObservableProperty] ResidenceDto? selectedResidence;

        [ObservableProperty] List<BatimentDto> batiments = new();
        [ObservableProperty] BatimentDto? selectedBatiment;

        public LotEditViewModel(ILotsApi lotsApi, IResidencesApi resApi, IBatimentsApi batApi)
        {
            _lotsApi = lotsApi; _resApi = resApi; _batApi = batApi;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (!Guid.TryParse(Id, out var guid)) return;

            Residences = await _resApi.GetAllAsync();

            var dto = await _lotsApi.GetByIdAsync(guid);
            NumeroLot = dto.NumeroLot ?? "";
            Type = dto.Type ?? "";
            Surface = dto.Surface;

            SelectedResidence = Residences.FirstOrDefault(r => r.Id == dto.ResidenceId);

            if (SelectedResidence is not null)
            {
                var all = await _batApi.GetAllAsync();
                Batiments = all.Where(b => b.ResidenceId == SelectedResidence.Id).ToList();
            }
            else
            {
                Batiments = new();
            }

            SelectedBatiment = dto.BatimentId.HasValue
                ? Batiments.FirstOrDefault(b => b.Id == dto.BatimentId.Value)
                : null;
        }

        [RelayCommand]
        public async Task ResidenceChangedAsync()
        {
            SelectedBatiment = null;
            Batiments = new();
            if (SelectedResidence is null) return;

            var all = await _batApi.GetAllAsync();
            Batiments = all.Where(b => b.ResidenceId == SelectedResidence.Id).ToList();
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (!Guid.TryParse(Id, out var guid)) return;

            if (string.IsNullOrWhiteSpace(NumeroLot) || string.IsNullOrWhiteSpace(Type) || SelectedResidence is null)
            {
                await Shell.Current.DisplayAlert("Validation", "Numéro, Type et Résidence sont obligatoires.", "OK");
                return;
            }

            var resId = SelectedResidence.Id;
            if (resId == Guid.Empty && !string.IsNullOrWhiteSpace(SelectedResidence.Nom))
            {
                var looked = await _resApi.LookupIdAsync(SelectedResidence.Nom); // Guid
                resId = looked;
            }

            Guid? batId = SelectedBatiment != null ? SelectedBatiment.Id : (Guid?)null;
            if (batId is null && SelectedBatiment is not null && !string.IsNullOrWhiteSpace(SelectedBatiment.Nom))
            {
                batId = await _batApi.ResolveIdAsync(SelectedBatiment.Nom); // Guid?
            }

            try
            {
                await _lotsApi.UpdateAsync(guid, new UpdateLotDto
                {
                    NumeroLot = NumeroLot.Trim(),
                    Type = Type.Trim(),
                    Surface = Surface,
                    ResidenceId = resId,
                    BatimentId = batId
                });

                WeakReferenceMessenger.Default.Send(new LotChangedMessage(true));
                await Shell.Current.DisplayAlert("Succès", "Lot modifié.", "OK");
                await Shell.Current.GoToAsync($"lot-details?id={guid:D}");
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Erreur API", ex.Content ?? ex.Message, "OK");
            }
        }

        [RelayCommand] private Task CancelAsync() => Shell.Current.GoToAsync("//lots");
    }
}
