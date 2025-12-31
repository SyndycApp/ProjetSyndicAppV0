using SyndicApp.Domain.Enums.Assemblees;


namespace SyndicApp.Application.DTOs.Assemblees
{
    public record CreateAssembleeDto(
    string Titre,
    TypeAssemblee Type,
    DateTime DateDebut,
    DateTime DateFin,
    Guid ResidenceId
);

    public record AssembleeDto(
        Guid Id,
        string Titre,
        TypeAssemblee Type,
        StatutAssemblee Statut,
        DateTime DateDebut,
        DateTime DateFin,
        int Annee
    );

}
