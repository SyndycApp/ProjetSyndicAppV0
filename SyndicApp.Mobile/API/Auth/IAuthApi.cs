using Refit;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Api;

public record LoginRequest(string Email, string Password);
public record LoginResponse(string token);
public record RegisterRequest(string Email, string Password, string FullName);

public interface IAuthApi
{
    [Post("/api/Auth/login")] Task<LoginResponse> Login([Body] LoginRequest req);
    [Post("/api/Auth/register")] Task<object> Register([Body] RegisterRequest req);
    [Get("/api/Auth/me")] Task<object> Me();
    [Get("/api/Auth")] Task<object> Ping();
    [Post("/api/Auth/logout")] Task<object> Logout();
}
