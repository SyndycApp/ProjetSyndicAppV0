// SyndicApp.Infrastructure/Services/Finances/ChargeService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Finances;
using SyndicApp.Domain.Entities.Finances;
using SyndicApp.Application.Interfaces.Finances;

namespace SyndicApp.Infrastructure.Services.Finances
{
    public class ChargeService : IChargeService
    {
        private readonly ApplicationDbContext _db;
        public ChargeService(ApplicationDbContext db) => _db = db;

        public async Task<IReadOnlyList<ChargeDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Charges.AsNoTracking()
                .Select(c => new ChargeDto
                {
                    Id = c.Id,
                    Nom = c.Nom,
                    Montant = c.Montant,
                    DateCharge = c.DateCharge,
                    ResidenceId = c.ResidenceId
                }).ToListAsync(ct);
        }

        public async Task<ChargeDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Charges.AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new ChargeDto
                {
                    Id = c.Id,
                    Nom = c.Nom,
                    Montant = c.Montant,
                    DateCharge = c.DateCharge,
                    ResidenceId = c.ResidenceId
                }).SingleOrDefaultAsync(ct);
        }

        public async Task<Guid> CreateAsync(CreateChargeDto dto, Guid? lotId = null, CancellationToken ct = default)
        {
           // CreateAsync(CreateChargeDto dto, Guid? lotId, ...)
if (dto.Montant < 0) throw new InvalidOperationException("Montant négatif interdit.");

if (!await _db.Residences.AnyAsync(r => r.Id == dto.ResidenceId, ct))
    throw new InvalidOperationException("Résidence introuvable.");

Guid? lotIdToSet = null;
if (lotId.HasValue)
{
    var lot = await _db.Lots.AsNoTracking()
                            .Where(l => l.Id == lotId.Value)
                            .Select(l => new { l.Id, l.ResidenceId })
                            .SingleOrDefaultAsync(ct);
    if (lot is null) throw new InvalidOperationException("Lot introuvable.");
    if (lot.ResidenceId != dto.ResidenceId)
        throw new InvalidOperationException("Le lot ne appartient pas à la résidence fournie.");

    lotIdToSet = lotId.Value;
}

var entity = new Charge
{
    Nom = dto.Nom.Trim(),
    Montant = dto.Montant,
    DateCharge = dto.DateCharge,
    ResidenceId = dto.ResidenceId,
    LotId = lotIdToSet            // <— direct, pas de EF.Property("LotId")
};

_db.Charges.Add(entity);
await _db.SaveChangesAsync(ct);
return entity.Id;

        }

        public async Task<bool> UpdateAsync(Guid id, UpdateChargeDto dto, Guid? lotId = null, CancellationToken ct = default)
        {
            var c = await _db.Charges.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (c is null) return false;
            if (dto.Montant < 0) throw new InvalidOperationException("Montant doit être ≥ 0.");

            c.Nom = dto.Nom?.Trim() ?? c.Nom;
            c.Montant = dto.Montant;
            c.DateCharge = dto.DateCharge;
            c.ResidenceId = dto.ResidenceId;

            _db.Entry(c).Property<Guid?>("LotId").CurrentValue = lotId;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var c = await _db.Charges.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (c is null) return false;
            _db.Charges.Remove(c);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
