using SyndicApp.Mobile.ViewModels.AppelVocal;

namespace SyndicApp.Mobile.Views.AppelVocal;

public partial class WebRtcCallPage : ContentPage
{
    private readonly WebRtcCallViewModel _vm;

    public WebRtcCallPage(WebRtcCallViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        CallWebView.Source = "webrtc_call.html";

        await Task.Delay(500); // laisse charger le HTML

        await CallWebView.EvaluateJavaScriptAsync(_vm.GetStartScript());
    }
}
