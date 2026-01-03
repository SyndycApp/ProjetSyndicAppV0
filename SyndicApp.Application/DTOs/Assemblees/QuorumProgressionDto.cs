namespace SyndicApp.Application.DTOs.Assemblees
{
    public record QuorumProgressionDto(
    decimal TotalTantiemes,
    decimal TantiemesRepresentes,
    decimal Pourcentage,
    bool QuorumAtteint
);
}
