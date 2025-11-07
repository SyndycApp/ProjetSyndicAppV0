// Services/TokenStore.cs
using Microsoft.Maui.Storage;

namespace SyndicApp.Mobile.Services;

public class TokenStore
{
    private const string TokenKey = "auth_token";
    private const string RoleKey = "user_role";

    public void SaveToken(string token) => Preferences.Set(TokenKey, token);
    public string? GetToken() => Preferences.Get(TokenKey, null);
    public void ClearToken() => Preferences.Remove(TokenKey);

    public void SaveRole(string role) => Preferences.Set(RoleKey, role);
    public string? GetRole() => Preferences.Get(RoleKey, null);
    public bool IsSyndic() => string.Equals(GetRole(), "Syndic", StringComparison.OrdinalIgnoreCase);

    public void Clear()
    {
        SecureStorage.Remove(TokenKey);
        Preferences.Remove(RoleKey);
    }
}
