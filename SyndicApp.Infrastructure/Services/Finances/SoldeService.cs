// SyndicApp.Infrastructure/Services/Finances/SoldeService.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Finances;
using SyndicApp.Application.Interfaces.Finances;

namespace SyndicApp.Infrastructure.Services.Finances
{
    public class SoldeService : ISoldeService
    {
        private readonly ApplicationDbContext _db;
        public SoldeService(ApplicationDbContext db) => _db = db;

        public async Task<SoldeLotDto> GetSoldeLotAsync(Guid lotId, CancellationToken ct = default)
        {
            var lot = await _db.Lots.AsNoTracking().FirstOrDefaultAsync(l => l.Id == lotId, ct);
            if (lot is null) throw new InvalidOperationException("Lot introuvable.");

            var appels = _db.AppelsDeFonds.AsNoTracking().Where(a => a.ResidenceId == lot.ResidenceId);
            var nbLots = await _db.Lots.CountAsync(l => l.ResidenceId == lot.ResidenceId, ct);
            if (nbLots == 0) throw new InvalidOperationException("Résidence sans lots.");

            var du = await appels.SumAsync(a => (decimal?)(a.MontantTotal / nbLots)) ?? 0m;

            var userIds = _db.AffectationsLots.AsNoTracking()
                .Where(a => a.LotId == lotId && a.DateFin == null)
                .Select(a => a.UserId);

            var paye = await _db.Paiements
                .Where(p => userIds.Contains(p.UserId) && _db.AppelsDeFonds.Any(a => a.Id == p.AppelDeFondsId && a.ResidenceId == lot.ResidenceId))
                .SumAsync(p => (decimal?)p.Montant, ct) ?? 0m;

            return new SoldeLotDto
            {
                LotId = lot.Id,
                NumeroLot = lot.NumeroLot,
                Du = Math.Round(du, 2),
                Paye = Math.Round(paye, 2)
            };
        }

        public async Task<SoldeResidenceDto> GetSoldeResidenceAsync(Guid residenceId, CancellationToken ct = default)
        {
            var lots = _db.Lots.AsNoTracking().Where(l => l.ResidenceId == residenceId);
            var nbLots = await lots.CountAsync(ct);

            var appels = _db.AppelsDeFonds.AsNoTracking().Where(a => a.ResidenceId == residenceId);
            var duTotal = await appels.SumAsync(a => (decimal?)a.MontantTotal, ct) ?? 0m;

            var payeTotal = await _db.Paiements
                .Where(p => _db.AppelsDeFonds.Any(a => a.Id == p.AppelDeFondsId && a.ResidenceId == residenceId))
                .SumAsync(p => (decimal?)p.Montant, ct) ?? 0m;

            var dto = new SoldeResidenceDto
            {
                ResidenceId = residenceId,
                NbLots = nbLots,
                DuTotal = Math.Round(duTotal, 2),
                PayeTotal = Math.Round(payeTotal, 2)
            };

            // détails lots (optionnel mais utile)
            var lotIds = await lots.Select(l => l.Id).ToListAsync(ct);
            foreach (var lid in lotIds)
                dto.Details.Add(await GetSoldeLotAsync(lid, ct));

            return dto;
        }
    }
}
