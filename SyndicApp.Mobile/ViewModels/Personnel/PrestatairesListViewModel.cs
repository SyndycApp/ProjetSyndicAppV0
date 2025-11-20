using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile.ViewModels.Personnel
{
    public partial class PrestatairesListViewModel : ObservableObject
    {
        private readonly IPrestatairesApi _api;

        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool isRefreshing;
        [ObservableProperty] private string? searchText;

        public ObservableCollection<PrestataireDto> Items { get; } = new();

        public PrestatairesListViewModel(IPrestatairesApi api)
        {
            _api = api;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Items.Clear();
                var list = await _api.GetAllAsync(SearchText);
                foreach (var p in list.OrderBy(x => x.Nom))
                    Items.Add(p);
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

        partial void OnSearchTextChanged(string? value)
        {
            // On recharge la liste avec le filtre serveur
            LoadCommand.Execute(null);
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
                await Shell.Current.GoToAsync("prestataire-create");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Navigation", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task GoToDetailsAsync(Guid id)
        {
            try
            {
                await Shell.Current.GoToAsync($"prestataire-details?id={id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Navigation", ex.Message, "OK");
            }
        }
    }
}
