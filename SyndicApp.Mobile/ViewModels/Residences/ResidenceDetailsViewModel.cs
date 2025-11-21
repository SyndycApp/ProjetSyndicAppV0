using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Common.Messages;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Residences
{
    [QueryProperty(nameof(Id), "id")]
    public partial class ResidenceDetailsViewModel : ObservableObject
    {
        private readonly IResidencesApi _residencesApi;
        private readonly ILotsApi _lotsApi;

        [ObservableProperty] private string id = "";
        [ObservableProperty] private ResidenceDto? residence;

        public ObservableCollection<LotDto> Lots { get; } = new();

        public ResidenceDetailsViewModel(IResidencesApi residencesApi, ILotsApi lotsApi)
        {
            _residencesApi = residencesApi;
            _lotsApi = lotsApi;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;

            try
            {
                Residence = await _residencesApi.GetByIdAsync(Id);

                Lots.Clear();
                if (Guid.TryParse(Id, out var resId))
                {
                    var lots = await _lotsApi.GetByResidenceAsync(resId);
                    foreach (var lot in lots)
                        Lots.Add(lot);
                }
            }
            catch (ApiException ex)
            {
                var msg = string.IsNullOrWhiteSpace(ex.Content)
                    ? ex.Message
                    : ex.Content;

                await Shell.Current.DisplayAlert("Erreur API", msg, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task EditAsync()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;
            await Shell.Current.GoToAsync($"residence-edit?id={Id}");
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;

            var ok = await Shell.Current.DisplayAlert(
                "Suppression", "Supprimer cette résidence ?", "Oui", "Non");
            if (!ok) return;

            try
            {
                var guid = Guid.Parse(Id);
                await _residencesApi.DeleteAsync(guid);

                WeakReferenceMessenger.Default.Send(new ResidenceChangedMessage(true));

                await Shell.Current.GoToAsync("//residences");
            }
            catch (ApiException ex)
            {
                var msg = string.IsNullOrWhiteSpace(ex.Content)
                    ? ex.Message
                    : ex.Content;

                await Shell.Current.DisplayAlert("Erreur API", msg, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }
    }
}
