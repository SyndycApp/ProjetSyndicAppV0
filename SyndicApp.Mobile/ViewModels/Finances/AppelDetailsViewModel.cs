using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances
{
    [QueryProperty(nameof(Id), "id")]
    public partial class AppelDetailsViewModel : ObservableObject
    {
        private readonly IAppelsApi _api;

        [ObservableProperty] private string id = string.Empty;
        [ObservableProperty] private AppelDeFondsDto? appel;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool isSyndic;

        public AppelDetailsViewModel(IAppelsApi api)
        {
            _api = api;
            IsSyndic = Preferences.Get("user_role", "").ToLowerInvariant().Contains("syndic");
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            Appel = await _api.GetByIdAsync(Id);
        }

        [RelayCommand]
        public async Task EditAsync()
        {
            if (!IsSyndic) return;

            await Shell.Current.GoToAsync($"appel-edit?id={Id}");
        }

        [RelayCommand]
        public async Task CloturerAsync()
        {
            if (!IsSyndic) return;

            await _api.CloturerAsync(Id);
            await LoadAsync();
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (!IsSyndic) return;

            await _api.DeleteAsync(Id);
            await Shell.Current.GoToAsync("//appels");
        }
    }
}
