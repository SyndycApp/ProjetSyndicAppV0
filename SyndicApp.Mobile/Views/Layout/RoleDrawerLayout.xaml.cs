using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace SyndicApp.Mobile.Views.Layout
{
    public partial class RoleDrawerLayout : ContentView
    {
        private bool _isOpen;

        // ====== SLOT pour le contenu des pages ======
        public static readonly BindableProperty MainContentProperty =
            BindableProperty.Create(
                nameof(MainContent),
                typeof(View),
                typeof(RoleDrawerLayout),
                default(View),
                propertyChanged: OnMainContentChanged);

        public View MainContent
        {
            get => (View)GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }

        private static void OnMainContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RoleDrawerLayout)bindable;

            // Rien à faire si le layout n'est pas encore initialisé
            if (layout.MainHost == null)
                return;

            layout.MainHost.Children.Clear();

            if (newValue is View view)
            {
                // très important : on met le même BindingContext que la page
                view.BindingContext = layout.BindingContext;
                layout.MainHost.Children.Add(view);
            }
        }

        // ====== TITLE BINDABLE PROPERTY (PageTitle) ======
        public static readonly BindableProperty PageTitleProperty =
            BindableProperty.Create(
                nameof(PageTitle),
                typeof(string),
                typeof(RoleDrawerLayout),
                "SyndicApp",
                propertyChanged: OnPageTitleChanged);

        public string PageTitle
        {
            get => (string)GetValue(PageTitleProperty);
            set => SetValue(PageTitleProperty, value);
        }

        private static void OnPageTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RoleDrawerLayout)bindable;

            if (layout.TitleLabel != null && newValue is string title)
            {
                layout.TitleLabel.Text = title;
            }
        }

        // ====== Hook "page appearing" pour les classes dérivées ======
        protected virtual void OnAppearing()
        {
            // par défaut : rien
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            if (Handler != null)
            {
                // Le layout est attaché à l’UI → on appelle le hook
                OnAppearing();
            }
        }

        public RoleDrawerLayout()
        {
            InitializeComponent();

            SizeChanged += RoleDrawerLayout_SizeChanged;

            ApplyRoleVisibility();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            // Quand le BindingContext change, on le pousse aussi dans le contenu
            if (MainContent is View v)
            {
                v.BindingContext = BindingContext;
            }
        }

        private void RoleDrawerLayout_SizeChanged(object? sender, EventArgs e)
        {
            var width = Width > 0
                ? Width
                : Application.Current?.Windows[0]?.Page?.Width ?? 360;

            Drawer.WidthRequest = width;
            if (!_isOpen)
                Drawer.TranslationX = -width;
        }

        // ====== RÔLES ======
        private void ApplyRoleVisibility()
        {
            try
            {
                var role = Preferences.Get("user_role", null)?.Trim() ?? string.Empty;
                var roleLower = role.ToLowerInvariant();

                bool isSyndic = roleLower.Contains("syndic");
                bool isCopro = roleLower.Contains("copro");

                SectionResidencesLabel.IsVisible = isSyndic;
                BtnMenuResidences.IsVisible = isSyndic;
                BtnMenuBatiments.IsVisible = isSyndic;
                BtnMenuLots.IsVisible = isSyndic;
                BtnMenuAffectations.IsVisible = isSyndic;

                SectionFinancesLabel.IsVisible = isSyndic || isCopro;
                BtnMenuAppels.IsVisible = isSyndic || isCopro;
                BtnMenuCharges.IsVisible = isSyndic;
                BtnMenuPaiements.IsVisible = isSyndic || isCopro;
            }
            catch
            {
                // en cas de problème on laisse tout visible
            }
        }

        // ====== ANIMATIONS DU DRAWER ======

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
            if (sender is Button b && b.CommandParameter is string route && !string.IsNullOrWhiteSpace(route))
            {
                await CloseDrawerAsync();
                try
                {
                    await Shell.Current.GoToAsync(route);
                }
                catch
                {
                    // ignore si route inconnue
                }
            }
        }
    }
}
