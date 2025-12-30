using SyndicApp.Application.DTOs.Personnel;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPrestataireAnalyticsService
    {
        Task<PrestataireStatsDto> GetStatsAsync(Guid prestataireId, DateOnly from, DateOnly to);
    }
}
