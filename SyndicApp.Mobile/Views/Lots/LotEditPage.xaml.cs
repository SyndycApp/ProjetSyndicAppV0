using Microsoft.Maui.Controls;
using SyndicApp.Mobile.ViewModels.Lots;
using System;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Views.Lots
{
    public partial class LotEditPage : ContentPage
    {
        private bool _isOpen;

        public LotEditPage() : this(ServiceHelper.Services.GetRequiredService<LotEditViewModel>()) { }

        public LotEditPage(LotEditViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

            // Charge les données du lot + listes au premier affichage
            Loaded += async (_, __) => await vm.LoadAsync();
        }

        private async void ResidenceChanged(object sender, EventArgs e)
        {
            if (BindingContext is LotEditViewModel vm)
                await vm.ResidenceChangedAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var w = this.Width > 0 ? this.Width : Application.Current?.Windows[0]?.Page?.Width ?? 360;
            Drawer.WidthRequest = w;
            Drawer.TranslationX = -w;

            Backdrop.InputTransparent = true;
            Backdrop.Opacity = 0;
            _isOpen = false;
        }

        protected override void OnSizeAllocated(double w, double h)
        {
            base.OnSizeAllocated(w, h);
            if (w > 0)
            {
                Drawer.WidthRequest = w;
                if (!_isOpen) Drawer.TranslationX = -w;
            }
        }

        private async void OpenDrawer_Clicked(object s, EventArgs e)
        {
            if (_isOpen) return;
            _isOpen = true;
            Backdrop.InputTransparent = false;
            await Backdrop.FadeTo(1, 160, Easing.CubicOut);
            await Drawer.TranslateTo(0, 0, 220, Easing.CubicOut);
        }

        private async void CloseDrawer_Clicked(object s, EventArgs e) => await CloseDrawerAsync();
        private async void Backdrop_Tapped(object s, TappedEventArgs e) => await CloseDrawerAsync();

        private async Task CloseDrawerAsync()
        {
            if (!_isOpen) return;
            _isOpen = false;
            await Drawer.TranslateTo(-Drawer.Width, 0, 220, Easing.CubicIn);
            await Backdrop.FadeTo(0, 140, Easing.CubicIn);
            Backdrop.InputTransparent = true;
        }

        private async void Back_Clicked(object s, EventArgs e) => await Shell.Current.GoToAsync("..");

        private async void OnMenuItemClicked(object s, EventArgs e)
        {
            if (s is Button b && b.CommandParameter is string r && !string.IsNullOrWhiteSpace(r))
            {
                await CloseDrawerAsync();
                await Shell.Current.GoToAsync(r);
            }
        }
    }
}
