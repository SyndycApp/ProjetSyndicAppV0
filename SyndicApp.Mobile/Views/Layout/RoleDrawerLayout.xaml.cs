using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile.Views.Layout
{
    public partial class RoleDrawerLayout : ContentView
    {
        private const uint AnimationDuration = 200;

        // === MainContent bindable property (utilisée par .MainContent dans les XAML) ===
        public static readonly BindableProperty MainContentProperty =
            BindableProperty.Create(
                nameof(MainContent),
                typeof(View),
                typeof(RoleDrawerLayout),
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
            {
                layout.MainHost.Children.Add(view);
            }
        }

        public RoleDrawerLayout()
        {
            InitializeComponent();

            // Initial state of drawer / backdrop
            Loaded += (_, _) =>
            {
                Drawer.TranslationX = -Drawer.Width;
                Backdrop.Opacity = 0;
                Backdrop.InputTransparent = true;
            };
        }

        public void SetPageTitle(string title)
        {
            TitleLabel.Text = title;
        }

        private async void OpenDrawer_Clicked(object sender, EventArgs e)
        {
            await OpenDrawerAsync(true);
        }

        private async void CloseDrawer_Clicked(object sender, EventArgs e)
        {
            await CloseDrawerAsync(true);
        }

        private async void Backdrop_Tapped(object sender, EventArgs e)
        {
            await CloseDrawerAsync(true);
        }

        public async Task OpenDrawerAsync(bool animate)
        {
            Backdrop.InputTransparent = false;

            if (animate)
            {
                await Task.WhenAll(
                    Drawer.TranslateTo(0, 0, AnimationDuration, Easing.CubicOut),
                    Backdrop.FadeTo(1, AnimationDuration, Easing.CubicOut)
                );
            }
            else
            {
                Drawer.TranslationX = 0;
                Backdrop.Opacity = 1;
            }
        }

        public async Task CloseDrawerAsync(bool animate)
        {
            if (animate)
            {
                await Task.WhenAll(
                    Drawer.TranslateTo(-Drawer.Width, 0, AnimationDuration, Easing.CubicIn),
                    Backdrop.FadeTo(0, AnimationDuration, Easing.CubicIn)
                );
            }
            else
            {
                Drawer.TranslationX = -Drawer.Width;
                Backdrop.Opacity = 0;
            }

            Backdrop.InputTransparent = true;
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            if (sender is not Button button)
                return;

            var route = button.CommandParameter as string;
            if (string.IsNullOrWhiteSpace(route))
                return;

            await CloseDrawerAsync(true);
            await Shell.Current.GoToAsync(route);
        }
    }
}
