using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Refit;


namespace SyndicApp.Mobile.ViewModels.Auth;


public partial class PasswordViewModel : BaseViewModel
{
    private readonly IPasswordApi _api;

    public PasswordViewModel(IPasswordApi api) => _api = api;

   
    public PasswordViewModel() : this(ServiceHelper.GetRequiredService<IPasswordApi>()) { }

    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private string token = string.Empty;
    [ObservableProperty] private string newPassword = string.Empty;



    [RelayCommand]
    private async Task ForgotAsync()
    {
        try { IsBusy = true; await _api.Forgot(new { email = Email }); }
        catch (ApiException ex) { Error = ex.Content ?? ex.Message; }
        finally { IsBusy = false; }
    }


    [RelayCommand]
    private async Task ResetAsync()
    {
        try { IsBusy = true; await _api.Reset(new { token = Token, password = NewPassword }); }
        catch (ApiException ex) { Error = ex.Content ?? ex.Message; }
        finally { IsBusy = false; }
    }
}