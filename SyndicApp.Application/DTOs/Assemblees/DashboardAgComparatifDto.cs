namespace SyndicApp.Application.DTOs.Assemblees;

public record DashboardAgComparatifDto(
    int Annee,
    int NombreAg,
    decimal TauxParticipation,
    decimal TauxQuorum,
    decimal TauxAdoptionResolutions
);

public record ParticipationParAgDto(
    string TitreAg,
    decimal TauxParticipation
);

public record RepartitionVotesDto(
    decimal Pour,
    decimal Contre,
    decimal Abstention
);