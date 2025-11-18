using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    [QueryProperty(nameof(DevisId), "id")]
    public partial class DevisTravauxDecisionViewModel : ObservableObject
    {
        private readonly IDevisTravauxApi _devisApi;
        private readonly IAccountApi _accountApi;

        [ObservableProperty] private string? devisId;
        [ObservableProperty] private bool isBusy;

        [ObservableProperty] private string statut = "EnAttente";
        [ObservableProperty] private string commentaire = string.Empty;

        public DevisTravauxDecisionViewModel(IDevisTravauxApi devisApi, IAccountApi accountApi)
        {
            _devisApi = devisApi;
            _accountApi = accountApi;
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (IsBusy) return;
            if (!Guid.TryParse(DevisId, out var guid)) return;

            IsBusy = true;
            try
            {
                var me = await _accountApi.MeAsync();
                Guid auteurId = me.Id;

                var req = new DevisTravauxDecisionRequest
                {
                    Statut = Statut,
                    AuteurId = auteurId,
                    Commentaire = Commentaire,
                    DateDecision = DateTime.UtcNow
                };

                await _devisApi.DecideAsync(guid, req);

                await Shell.Current.DisplayAlert("Succès", "Décision enregistrée.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public Task CancelAsync() => Shell.Current.GoToAsync("..");
    }
}
