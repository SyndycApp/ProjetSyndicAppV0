using SyndicApp.Application.Interfaces.Assemblees;
using Microsoft.EntityFrameworkCore;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class QuorumService : IQuorumService
    {
        private readonly ApplicationDbContext _db;

        public async Task<bool> QuorumAtteintAsync(Guid assembleeId)
        {
            var total = await _db.PresenceAss
                .Where(p => p.AssembleeGeneraleId == assembleeId)
                .SumAsync(p => p.Tantiemes);

            return total >= 50; 
        }
    }

}
