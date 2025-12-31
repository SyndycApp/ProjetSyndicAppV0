using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class VoteService : IVoteService
    {
        private readonly ApplicationDbContext _db;

        public VoteService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task VoteAsync(Guid userId, VoteDto dto)
        {
            var exists = await _db.Votes
                .AnyAsync(v => v.ResolutionId == dto.ResolutionId && v.UserId == userId);

            if (exists)
                throw new InvalidOperationException("Vote déjà existant");

            var vote = new Vote
            {
                ResolutionId = dto.ResolutionId,
                UserId = userId,
                LotId = dto.LotId,
                Choix = dto.Choix,
                PoidsVote = 1, // TODO : récupérer tantièmes
                DateVote = DateTime.UtcNow
            };

            _db.Votes.Add(vote);
            await _db.SaveChangesAsync();
        }
    }

}
