using SyndicApp.Domain.Enums.Assemblees;
namespace SyndicApp.Application.DTOs.Assemblees
{
    public record VoteDto(
    Guid ResolutionId,
    Guid LotId,
    ChoixVote Choix
);

}
