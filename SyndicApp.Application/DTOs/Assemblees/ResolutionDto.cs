using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record CreateResolutionDto(
    int Numero,
    string Titre,
    string Description
);

    public record ResolutionDto(
        Guid Id,
        int Numero,
        string Titre,
        string Description,
        StatutResolution Statut
    );

}
