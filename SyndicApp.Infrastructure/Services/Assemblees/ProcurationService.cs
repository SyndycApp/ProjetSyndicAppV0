using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;


namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class ProcurationService : IProcurationService
    {
        private readonly ApplicationDbContext _db;
        private const int MAX_PROCURATIONS = 3;


        public ProcurationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task DonnerProcurationAsync(Guid userId, CreateProcurationDto dto)
        {
            var count = await _db.Procurations
                .CountAsync(p => p.AssembleeGeneraleId == dto.AssembleeId
                              && p.MandataireId == dto.MandataireId);

            if (count >= MAX_PROCURATIONS)
                throw new InvalidOperationException("Limite légale atteinte");

            _db.Procurations.Add(new Procuration
            {
                AssembleeGeneraleId = dto.AssembleeId,
                DonneurId = userId,
                MandataireId = dto.MandataireId,
                LotId = dto.LotId,
                DateCreation = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }
    }

}
