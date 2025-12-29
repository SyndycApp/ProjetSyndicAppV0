using Refit;
using SyndicApp.Mobile.Models;


namespace SyndicApp.Mobile.Api;

public interface IPresenceApi
{
    [Post("/api/presence/start")]
    Task StartAsync([Body] StartPresenceDto dto);

    [Post("/api/presence/end")]
    Task EndAsync();

    [Get("/api/presence/me")]
    Task<List<PresenceDto>> GetMyHistoryAsync();
}
