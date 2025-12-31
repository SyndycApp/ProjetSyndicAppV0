using SyndicApp.Domain.Enums.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record VoteDto(
    Guid ResolutionId,
    Guid LotId,
    ChoixVote Choix
);

}
