using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.ViewModels.Finances;

public partial class AppelCreateViewModel : ObservableObject
{
    private readonly IAppelsApi _api;

    // ⚠️ Utilise bien les noms attendus par ton API/DTO
    [ObservableProperty] private string? description;
    [ObservableProperty] private DateTime dateEmission = DateTime.Today;
    [ObservableProperty] private string residenceId = string.Empty;

    // Saisie utilisateur côté UI (string -> on parse nous-mêmes)
    [ObservableProperty] private string montantTotalText = string.Empty;

    [ObservableProperty] private bool isBusy;

    public AppelCreateViewModel(IAppelsApi api)
    {
        _api = api;
    }

    [RelayCommand]
    private async Task CreateAsync()
    {
        if (IsBusy) return;

        // Validation minimale côté client
        if (string.IsNullOrWhiteSpace(Description))
        {
            await Shell.Current.DisplayAlert("Champs requis", "La description est obligatoire.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(ResidenceId))
        {
            await Shell.Current.DisplayAlert("Champs requis", "La résidence est obligatoire.", "OK");
            return;
        }

        // Parse du montant (accepte , et .)
        if (!TryParseDecimal(MontantTotalText, out var montant))
        {
            await Shell.Current.DisplayAlert("Montant invalide", "Saisis un montant valide (ex: 1234,56).", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            // ⚠️ Adapte ce DTO aux propriétés exactes attendues par ton API Create
            var payload = new AppelDeFondsDto
            {
                Description = Description,
                DateEmission = DateEmission,
                ResidenceId = ResidenceId,  
                MontantTotal = montant
            };

            // POST
            var created = await _api.CreateAsync(payload);

            // Navigation : vers la liste ou les détails si l’API renvoie l’Id
            if (!string.IsNullOrWhiteSpace(created?.Id))
                await Shell.Current.GoToAsync($"appel-details?id={created.Id}");
            else
                await Shell.Current.GoToAsync("//appels");
        }
        catch (ApiException ex)
        {
            // Affiche le détail de la validation serveur
            var message = string.IsNullOrWhiteSpace(ex.Content) ? ex.Message : ex.Content;
            await Shell.Current.DisplayAlert("Erreur API (400)", message, "OK");
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

    private static bool TryParseDecimal(string? input, out decimal value)
    {
        // Essaye culture courante puis invariant, et remplace ,/. au besoin
        input = (input ?? string.Empty).Trim();
        input = input.Replace(' ', '\0');

        if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out value))
            return true;
        if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
            return true;

        // petite tolérance : remplacer virgule par point
        var swapped = input.Replace(',', '.');
        return decimal.TryParse(swapped, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
    }
}
