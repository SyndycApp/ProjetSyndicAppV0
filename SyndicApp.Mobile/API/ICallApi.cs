using Refit;
using SyndicApp.Mobile.Models;


namespace SyndicApp.Mobile.Api
{
    public interface ICallApi
    {
        [Post("/api/calls/start")]
        Task<CallDto> StartCallAsync([Body] StartCallRequest body);

        [Post("/api/calls/{callId}/end")]
        Task EndCallAsync(Guid callId);

        [Get("/api/calls/history")]
        Task<List<CallDto>> GetHistoryAsync();

        [Get("/api/calls/missed")]
        Task<List<CallDto>> GetMissedAsync();
    }
}
