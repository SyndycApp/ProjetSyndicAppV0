using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Api;
using Refit;

namespace SyndicApp.Mobile.ViewModels.Auth;


public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthApi _auth;
    private readonly TokenStore _tokens;


    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private string password = string.Empty;


    public LoginViewModel(IAuthApi auth, TokenStore tokens)
    { _auth = auth; _tokens = tokens; }


    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            IsBusy = true; Error = null;
            var res = await _auth.Login(new LoginRequest(Email, Password));
            await _tokens.SaveAsync(res.token);
            await Shell.Current.GoToAsync("//home");
        }
        catch (ApiException ex) { Error = ex.Content ?? ex.Message; }
        finally { IsBusy = false; }
    }
}