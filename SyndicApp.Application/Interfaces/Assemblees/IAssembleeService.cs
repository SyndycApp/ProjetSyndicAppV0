using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;


namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IAssembleeService
    {
        Task<Guid> CreateAsync(CreateAssembleeDto dto, Guid userId);
        Task PublishAsync(Guid assembleeId);
        Task CloseAsync(Guid assembleeId);
        Task<List<AssembleeDto>> GetUpcomingAsync(Guid residenceId);

        Task AnnulerAsync(Guid assembleeId);
        Task<Guid> DupliquerAsync(Guid assembleeId);

        Task MettreAJourStatutSiNecessaireAsync(AssembleeGenerale ag);

        Task<List<AssembleeDto>> GetHistoriqueAsync(Guid residenceId, AssembleeHistoriqueFilterDto filter);
    }
}

