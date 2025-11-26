using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile.Views.Layout
{
    public partial class RoleDrawerLayout : ContentView
    {
        // ========= MainContent (contenu de la page) =========
        public static readonly BindableProperty MainContentProperty =
            BindableProperty.Create(
                nameof(MainContent),
                typeof(View),
                typeof(RoleDrawerLayout),
                null,
                propertyChanged: OnMainContentChanged);

        public View MainContent
        {
            get => (View)GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }

        private static void OnMainContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RoleDrawerLayout)bindable;
            layout.MainHost.Children.Clear();

            if (newValue is View view)
                layout.MainHost.Children.Add(view);
        }

        private bool _isOpen;

        public RoleDrawerLayout()
        {
            InitializeComponent();

            // ⚠️ IMPORTANT :
            // On ne touche PAS à Drawer.TranslationX ni Backdrop ici.
            // On laisse les valeurs définies dans le XAML :
            //   Drawer.TranslationX = -1000 (fermé)
            //   Backdrop.Opacity = 0, InputTransparent = true
        }

        // ========= OUVERTURE / FERMETURE =========

        private async Task OpenDrawerAsync()
        {
            if (_isOpen)
                return;

            _isOpen = true;
            Backdrop.InputTransparent = false;

            // Le drawer est déjà en dehors de l'écran (XAML: -1000),
            // on l'anime juste vers 0.
            await Task.WhenAll(
                Drawer.TranslateTo(0, 0, 200, Easing.CubicOut),
                Backdrop.FadeTo(0.5, 200, Easing.CubicOut));
        }

        private async Task CloseDrawerAsync()
        {
            if (!_isOpen)
                return;

            _isOpen = false;

            // On l’anime vers la gauche, puis on désactive le backdrop.
            await Task.WhenAll(
                Drawer.TranslateTo(-Drawer.Width, 0, 200, Easing.CubicIn),
                Backdrop.FadeTo(0, 200, Easing.CubicIn));

            Backdrop.InputTransparent = true;
        }

        // ========= HANDLERS XAML =========

        private async void OpenDrawer_Clicked(object sender, EventArgs e)
        {
            await OpenDrawerAsync();
        }

        private async void CloseDrawer_Clicked(object sender, EventArgs e)
        {
            await CloseDrawerAsync();
        }

        private async void Backdrop_Tapped(object sender, TappedEventArgs e)
        {
            await CloseDrawerAsync();
        }

        // 🔴 IMPORTANT : on ferme le drawer AVANT la navigation
        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (btn.CommandParameter is not string route || string.IsNullOrWhiteSpace(route))
                return;

            // 1) Fermer le drawer sur la page actuelle
            await CloseDrawerAsync();

            // 2) Naviguer vers la nouvelle page
            try
            {
                await Shell.Current.GoToAsync(route);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
            }
        }
    }
}
