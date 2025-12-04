// Api/IAuthApi.cs  (public / sans bearer)
using Refit;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Api;
public interface IAuthApi
{
    [Post("/api/Auth/login")]
    Task<UserDto> LoginAsync([Body] LoginDto dto);

    [Post("/api/Auth/register")]
    Task<UserDto> RegisterAsync([Body] RegisterDto dto);

    [Get("/api/Auth")]
    Task<Result<List<UserDto>>> GetAllAsync();

    [Post("/api/Auth/register-from-prestataire-account")]
    Task<UserDto> RegisterFromPrestataireAccountAsync([Body] CreatePrestataireAccountRequest dto);
}

// Api/IAccountApi.cs  (protégé / avec bearer)

public interface IAccountApi
{
    [Get("/api/Auth/me")]
    Task<UserDto> MeAsync();

    [Post("/api/Auth/logout")]
    Task<ApiOkDto> LogoutAsync();
}
