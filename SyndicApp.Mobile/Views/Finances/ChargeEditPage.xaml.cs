using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances;

public partial class ChargeEditPage : ContentPage, IQueryAttributable
{
    public ChargeEditViewModel VM { get; }
    private bool _isOpen;

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

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var width = this.Width > 0
            ? this.Width
            : Application.Current?.Windows[0]?.Page?.Width ?? 360;

        Drawer.WidthRequest = width;
        Drawer.TranslationX = -width;
        Backdrop.InputTransparent = true;
        Backdrop.Opacity = 0;
        _isOpen = false;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (width > 0)
        {
            Drawer.WidthRequest = width;
            if (!_isOpen) Drawer.TranslationX = -width;
        }
    }

    private async void OpenDrawer_Clicked(object sender, EventArgs e)
    {
        if (_isOpen) return;
        _isOpen = true;

        Backdrop.InputTransparent = false;
        await Backdrop.FadeTo(1, 160, Easing.CubicOut);
        await Drawer.TranslateTo(0, 0, 220, Easing.CubicOut);
    }

    private async void CloseDrawer_Clicked(object sender, EventArgs e) => await CloseDrawerAsync();
    private async void Backdrop_Tapped(object sender, TappedEventArgs e) => await CloseDrawerAsync();

    private async Task CloseDrawerAsync()
    {
        if (!_isOpen) return;
        _isOpen = false;

        await Drawer.TranslateTo(-Drawer.Width, 0, 220, Easing.CubicIn);
        await Backdrop.FadeTo(0, 140, Easing.CubicIn);
        Backdrop.InputTransparent = true;
    }

    private async void OnMenuItemClicked(object sender, EventArgs e)
    {
        if (sender is Button b &&
            b.CommandParameter is string route &&
            !string.IsNullOrWhiteSpace(route))
        {
            await CloseDrawerAsync();
            await Shell.Current.GoToAsync(route);
        }
    }
}
