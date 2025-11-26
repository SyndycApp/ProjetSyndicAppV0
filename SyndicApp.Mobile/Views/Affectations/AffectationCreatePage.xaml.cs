using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SyndicApp.Mobile.ViewModels.Affectations;

namespace SyndicApp.Mobile.Views.Affectations
{
    public partial class AffectationCreatePage : ContentPage
    {
        public AffectationCreatePage(AffectationCreateViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Si l'utilisateur n'est pas syndic => on interdit l'accès
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var roleLower = role.ToLowerInvariant();
                bool isSyndic = roleLower.Contains("syndic");

                if (!isSyndic)
                {
                    await DisplayAlert("Accès refusé",
                        "Seul le syndic peut créer des affectations.",
                        "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }
            }
            catch
            {
                // en cas de bug, on laisse passer
            }

            // Charger les listes Users / Lots
            if (BindingContext is AffectationCreateViewModel vm)
            {
                try
                {
                    await vm.LoadAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erreur",
                        $"Impossible de charger les données.\n\n{ex.Message}",
                        "OK");
                }
            }
        }

        // Annuler = retour arrière
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur navigation",
                    $"Impossible de revenir en arrière.\n\n{ex.Message}",
                    "OK");
            }
        }

        // Enregistrer = on essaie d'appeler une méthode async du VM
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (BindingContext is not AffectationCreateViewModel vm)
                return;

            try
            {
                // On cherche une méthode async à appeler : CreateAsync ou SaveAsync
                var method =
                    vm.GetType().GetMethod("CreateAsync",
                        BindingFlags.Instance | BindingFlags.Public)
                    ?? vm.GetType().GetMethod("SaveAsync",
                        BindingFlags.Instance | BindingFlags.Public);

                if (method is null)
                {
                    await DisplayAlert("Erreur",
                        "Aucune méthode CreateAsync/SaveAsync trouvée dans le ViewModel.",
                        "OK");
                    return;
                }

                var result = method.Invoke(vm, null);

                if (result is Task task)
                    await task;

                // si tout va bien, on revient à la liste
                await Shell.Current.GoToAsync("..");
            }
            catch (TargetInvocationException tie) when (tie.InnerException is not null)
            {
                await DisplayAlert("Erreur",
                    $"Échec de l’enregistrement.\n\n{tie.InnerException.Message}",
                    "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur",
                    $"Échec de l’enregistrement.\n\n{ex.Message}",
                    "OK");
            }
        }
    }
}
