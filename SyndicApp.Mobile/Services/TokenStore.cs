using Microsoft.Maui.Storage;
using System.Threading.Tasks;

public class TokenStore
{
    const string Key = "jwt_token";
    public Task SaveAsync(string token) => SecureStorage.SetAsync(Key, token ?? "");
    public async Task<string?> GetAsync() => await SecureStorage.GetAsync(Key);
    public Task ClearAsync() => SecureStorage.SetAsync(Key, "");
}
