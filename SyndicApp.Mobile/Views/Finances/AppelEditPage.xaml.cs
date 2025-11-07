using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;
public partial class AppelEditPage : ContentPage
{
    public AppelEditViewModel VM { get; }
    public AppelEditPage(AppelEditViewModel vm)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
        Loaded += async (_, __) => await VM.LoadAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        // on réutilise la page Détails pour suppression (optionnel) :
        bool ok = await DisplayAlert("Suppression", "Supprimer cet appel ?", "Oui", "Non");
        if (!ok) return;

        // petite astuce : passer par l'API via un VM rapide
        await Shell.Current.GoToAsync($"appel-details?id={VM.Id}");
    }
}
