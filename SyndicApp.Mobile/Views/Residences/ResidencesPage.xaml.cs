using System;

namespace SyndicApp.Mobile.Views.Residences;


public partial class ResidencesPage : ContentPage
{
    public ResidencesPage() => InitializeComponent();
    private async void OnAppearing(object sender, EventArgs e)
    => await (BindingContext as ViewModels.Residences.ResidencesListViewModel)!.LoadAsync();
}