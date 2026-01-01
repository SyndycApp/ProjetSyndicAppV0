using SyndicApp.Application.DTOs.Assemblees;


namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IProcurationService
    {
        Task DonnerProcurationAsync(Guid userId, CreateProcurationDto dto);

        Task<List<ProcurationViewDto>> GetProcurationsAsync(Guid assembleeId);
    }
}
