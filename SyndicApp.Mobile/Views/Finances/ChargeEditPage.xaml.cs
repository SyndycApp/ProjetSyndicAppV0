using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class ChargeEditPage : ContentPage, IQueryAttributable
{
    public ChargeEditViewModel VM { get; }

    public ChargeEditPage(ChargeEditViewModel vm)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idObj) &&
            idObj is string idStr &&
            Guid.TryParse(idStr, out var id))
        {
            await VM.InitializeAsync(id);
        }
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
