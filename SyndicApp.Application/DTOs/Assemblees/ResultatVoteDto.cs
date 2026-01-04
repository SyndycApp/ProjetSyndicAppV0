namespace SyndicApp.Application.DTOs.Assemblees
{
    public record ResultatVoteDto(
       Guid ResolutionId,
       decimal TotalPour,
       decimal TotalContre,
       decimal TotalAbstention,
       decimal TotalExprime,
       bool EstAdoptee
   );
}
