using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.Services;

namespace SyndicApp.Mobile.ViewModels.Incidents
{
    public partial class DevisTravauxListViewModel : ObservableObject
    {
        private readonly IDevisTravauxApi _devisApi;
        private readonly IAccountApi _accountApi;
        private readonly TokenStore _tokenStore;

        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool isRefreshing;
        [ObservableProperty] private bool isSyndic;

        public ObservableCollection<DevisTravauxDto> Items { get; } = new();

        public DevisTravauxListViewModel(
            IDevisTravauxApi devisApi,
            IAccountApi accountApi,
            TokenStore tokenStore)
        {
            _devisApi = devisApi;
            _accountApi = accountApi;
            _tokenStore = tokenStore;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var me = await _accountApi.MeAsync();
                var role = me.Roles?.FirstOrDefault()?.Trim();
                IsSyndic = string.Equals(role, "Syndic", StringComparison.OrdinalIgnoreCase);

                Items.Clear();

                var list = await _devisApi.GetAllAsync(1, 50);

                foreach (var d in list.OrderByDescending(x => x.DateEmission))
                    Items.Add(d);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        public async Task RefreshAsync()
        {
            IsRefreshing = true;
            await LoadAsync();
        }

        [RelayCommand]
        public async Task GoToCreateAsync()
        {
            try
            {
                await Shell.Current.GoToAsync("devis-create");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Navigation", $"Erreur vers création devis : {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task GoToDetailsAsync(Guid id)
        {
            try
            {
                await Shell.Current.GoToAsync($"devis-details?id={id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Navigation", $"Erreur vers détails devis : {ex.Message}", "OK");
            }
        }
    }
}
