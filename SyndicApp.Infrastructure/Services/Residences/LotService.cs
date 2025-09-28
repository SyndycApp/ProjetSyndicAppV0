using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Application.Interfaces.Residences;
using SyndicApp.Domain.Entities.Residences;

namespace SyndicApp.Infrastructure.Services.Residences
{
    public class LotService : ILotService
    {
        private readonly ApplicationDbContext _db;
        public LotService(ApplicationDbContext db) => _db = db;

        public async Task<IReadOnlyList<LotDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Lots
                .AsNoTracking()
                .Select(l => new LotDto
                {
                    Id = l.Id,
                    NumeroLot = l.NumeroLot,
                    Type = l.Type,
                    Surface = l.Surface,
                    ResidenceId = l.ResidenceId,
                    // lit la FK "shadow" créée par le mapping
                    BatimentId = EF.Property<Guid?>(l, "BatimentId")
                })
                .ToListAsync(ct);
        }


        public async Task<IReadOnlyList<LotDto>> GetByResidenceAsync(Guid residenceId, CancellationToken ct = default)
        {
            var query =
                from l in _db.Lots.AsNoTracking()
                where l.ResidenceId == residenceId
                join b in _db.Batiments.AsNoTracking()
                    on EF.Property<Guid?>(l, "BatimentId") equals b.Id into jb
                from b in jb.DefaultIfEmpty()
                select new LotDto
                {
                    Id = l.Id,
                    NumeroLot = l.NumeroLot,
                    Type = l.Type,
                    Surface = l.Surface,
                    ResidenceId = l.ResidenceId,
                    BatimentId = b != null ? b.Id : (Guid?)null,
                    BatimentNom = b != null ? b.Nom : null
                };

            return await query.ToListAsync(ct);
        }

        public async Task<LotDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var l = await _db.Lots.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (l is null) return null;

            var entry = _db.Entry(l).Property<Guid?>("BatimentId");
            Guid? batId = entry.CurrentValue;   // <= changement ici
            string? batNom = null;

            if (batId.HasValue)
            {
                var b = await _db.Batiments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == batId.Value, ct);
                batNom = b?.Nom;
            }

            return new LotDto
            {
                Id = l.Id,
                NumeroLot = l.NumeroLot,
                Type = l.Type,
                Surface = l.Surface,
                ResidenceId = l.ResidenceId,
                BatimentId = batId,
                BatimentNom = batNom
            };
        }

        public async Task<Guid> CreateAsync(CreateLotDto dto, CancellationToken ct = default)
        {
            if (!await _db.Residences.AnyAsync(r => r.Id == dto.ResidenceId, ct))
                throw new InvalidOperationException("Résidence introuvable.");

            var entity = new Lot
            {
                NumeroLot = dto.NumeroLot?.Trim() ?? string.Empty,
                Type = dto.Type?.Trim() ?? string.Empty,
                Surface = dto.Surface,
                ResidenceId = dto.ResidenceId
            };

            _db.Lots.Add(entity);
            await _db.SaveChangesAsync(ct);

            if (dto.BatimentId.HasValue)
            {
                _db.Entry(entity).Property("BatimentId").CurrentValue = dto.BatimentId.Value;
                await _db.SaveChangesAsync(ct);
            }

            return entity.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateLotDto dto, CancellationToken ct = default)
        {
            var l = await _db.Lots.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (l is null) return false;

            if (!await _db.Residences.AnyAsync(r => r.Id == dto.ResidenceId, ct))
                throw new InvalidOperationException("Résidence introuvable.");

            l.NumeroLot = dto.NumeroLot?.Trim() ?? l.NumeroLot;
            l.Type = dto.Type?.Trim() ?? l.Type;
            l.Surface = dto.Surface;
            l.ResidenceId = dto.ResidenceId;

            // maj BatimentId (shadow)
            _db.Entry(l).Property("BatimentId").CurrentValue = dto.BatimentId;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var l = await _db.Lots.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (l is null) return false;

            _db.Lots.Remove(l);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
