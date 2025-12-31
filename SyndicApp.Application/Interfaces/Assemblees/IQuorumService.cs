using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IQuorumService
    {
        Task<bool> QuorumAtteintAsync(Guid assembleeId);
    }
}
