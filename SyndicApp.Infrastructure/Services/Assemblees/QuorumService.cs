using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class QuorumService : IQuorumService
    {
        private readonly ApplicationDbContext _db;
        private readonly IAssembleeAccessPolicy _accessPolicy;

        public QuorumService(ApplicationDbContext db, IAssembleeAccessPolicy accessPolicy)
        {
            _db = db;
            _accessPolicy = accessPolicy;
        }

        public async Task<QuorumProgressionDto> GetProgressionAsync(Guid assembleeId)
        {
            var assemblee = await _db.AssembleesGenerales
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == assembleeId);

            if (assemblee == null)
                throw new InvalidOperationException("Assemblée introuvable");

            var totalTantiemes = await _db.Lots
                .Where(l => l.ResidenceId == assemblee.ResidenceId)
                .SumAsync(l => l.Tantiemes);

            if (totalTantiemes == 0)
                return new QuorumProgressionDto(0, 0, 0, false);

            var tantiemesPresence = await _db.PresenceAss
                .Where(p => p.AssembleeGeneraleId == assembleeId)
                .SumAsync(p => p.Tantiemes);

            var tantiemesProcuration = await _db.Procurations
                .Where(p => p.AssembleeGeneraleId == assembleeId)
                .Join(
                    _db.Lots,
                    p => p.LotId,
                    l => l.Id,
                    (p, l) => l.Tantiemes
                )
                .SumAsync();

            var totalRepresentes = tantiemesPresence + tantiemesProcuration;

            var pourcentage = Math.Round(
                (totalRepresentes / totalTantiemes) * 100,
                2
            );

            var taux = _accessPolicy.GetTauxQuorumRequis(assemblee);
            var quorumAtteint = totalRepresentes >= (totalTantiemes * taux);

            return new QuorumProgressionDto(
                totalTantiemes,
                totalRepresentes,
                pourcentage,
                quorumAtteint
            );
        }

        public async Task<bool> QuorumAtteintAsync(Guid assembleeId)
        {
            // 1️⃣ Récupérer l’AG
            var assemblee = await _db.AssembleesGenerales
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == assembleeId);

            if (assemblee == null)
                throw new InvalidOperationException("Assemblée introuvable");

            // 2️⃣ Total des tantièmes de la résidence
            var totalTantiemes = await _db.Lots
                .Where(l => l.ResidenceId == assemblee.ResidenceId)
                .SumAsync(l => l.Tantiemes);

            if (totalTantiemes == 0)
                return false;

            // 3️⃣ Tantièmes présents physiquement / visio
            var tantiemesPresence = await _db.PresenceAss
                .Where(p => p.AssembleeGeneraleId == assembleeId)
                .SumAsync(p => p.Tantiemes);

            // 4️⃣ Tantièmes représentés par procuration
            var tantiemesProcuration = await _db.Procurations
                .Where(p => p.AssembleeGeneraleId == assembleeId)
                .Join(
                    _db.Lots,
                    p => p.LotId,
                    l => l.Id,
                    (p, l) => l.Tantiemes
                )
                .SumAsync();

            var totalRepresentes = tantiemesPresence + tantiemesProcuration;

            // 5️⃣ Règle légale standard : +50%
            return totalRepresentes >= (totalTantiemes / 2);
        }
    }
}
