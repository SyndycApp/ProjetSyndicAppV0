using SyndicApp.Application.DTOs.Assemblees;


namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IAssembleeService
    {
        Task<Guid> CreateAsync(CreateAssembleeDto dto, Guid userId);
        Task PublishAsync(Guid assembleeId);
        Task CloseAsync(Guid assembleeId);
        Task<List<AssembleeDto>> GetUpcomingAsync(Guid residenceId);
    }
}

