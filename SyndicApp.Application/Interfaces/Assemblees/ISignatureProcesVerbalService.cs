using SyndicApp.Application.DTOs.Assemblees;

namespace SyndicApp.Application.Interfaces.Assemblees;

public interface ISignatureProcesVerbalService
{
    Task DemarrerWorkflowAsync(Guid procesVerbalId, List<Guid> signatairesIds);

    Task SignerAsync(Guid procesVerbalId, Guid userId, string? commentaire);

    Task<ProcesVerbalEtatDto> GetEtatAsync(Guid procesVerbalId);
}
