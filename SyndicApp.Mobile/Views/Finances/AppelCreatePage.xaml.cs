using SyndicApp.Mobile.ViewModels.Finances;

namespace SyndicApp.Mobile.Views.Finances
{
    public partial class AppelCreatePage : ContentPage
    {
        public AppelCreatePage()
        {
            InitializeComponent();
            BindingContext = ServiceHelper.Get<AppelCreateViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AppelCreateViewModel vm && !vm.IsBusy)
            {
                vm.LoadResidencesCommand.Execute(null);
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//appels");
        }
    }
}
