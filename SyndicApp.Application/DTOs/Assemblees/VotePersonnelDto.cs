using SyndicApp.Domain.Enums.Assemblees;
namespace SyndicApp.Application.DTOs.Assemblees
{
    public record VotePersonnelDto(
        Guid ResolutionId,
        int NumeroResolution,
        string TitreResolution,
        ChoixVote Choix,
        decimal PoidsVote,
        DateTime DateVote
    );

}
