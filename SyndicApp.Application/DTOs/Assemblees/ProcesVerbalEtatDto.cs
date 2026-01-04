using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Application.DTOs.Assemblees;

public record ProcesVerbalEtatDto(
    Guid ProcesVerbalId,
    StatutProcesVerbal Statut,
    bool EstOfficiel,
    List<SignatureProcesVerbalDto> Signatures
);
