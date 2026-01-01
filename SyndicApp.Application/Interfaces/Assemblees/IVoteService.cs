using SyndicApp.Application.DTOs.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IVoteService
    {
        Task VoteAsync(Guid userId, VoteDto dto);

        Task<ResultatVoteDto> CalculerResultatAsync(Guid resolutionId);
    }

}
