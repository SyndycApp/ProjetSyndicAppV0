using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances
{
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

        private async void OnBackClicked(object? sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
