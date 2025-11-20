using Android.Locations;
using Android.Provider;
using Android.Service.QuickSettings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.ViewModels.Personnel
{
    public partial class PrestataireCreateViewModel : ObservableObject
    {
        private readonly IPrestatairesApi _api;

        public PrestataireCreateViewModel(IPrestatairesApi api)
        {
            _api = api;
        }

        [ObservableProperty] private string nom = string.Empty;
        [ObservableProperty] private string? typeService;
        [ObservableProperty] private string? email;
        [ObservableProperty] private string? telephone;
        [ObservableProperty] private string? adresse;
        [ObservableProperty] private string? notes;
        [ObservableProperty] private bool isBusy;

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (IsBusy) return;
            if (string.IsNullOrWhiteSpace(Nom))
            {
                await Shell.Current.DisplayAlert("Validation", "Le nom est obligatoire.", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                var dto = new PrestataireCreateRequest
                {
                    Nom = Nom.Trim(),
                    TypeService = TypeService,
                    Email = Email,
                    Telephone = Telephone,
                    Adresse = Adresse,
                    Notes = Notes
                };

                await _api.CreateAsync(dto);

                await Shell.Current.DisplayAlert("OK", "Prestataire créé.", "OK");
                await Shell.Current.GoToAsync("//prestataires");
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
        public async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
