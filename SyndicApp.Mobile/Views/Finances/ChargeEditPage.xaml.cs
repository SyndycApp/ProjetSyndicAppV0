using System;
using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class ChargeEditPage : ContentPage, IQueryAttributable
{
    private readonly ChargeEditViewModel _vm;

    public ChargeEditPage(ChargeEditViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idObj) && idObj is string idStr && Guid.TryParse(idStr, out var id))
        {
            await _vm.InitializeAsync(id);
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
