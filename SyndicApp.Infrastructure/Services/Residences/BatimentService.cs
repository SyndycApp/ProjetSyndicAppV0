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
    public class BatimentService : IBatimentService
    {
        private readonly ApplicationDbContext _db;
        public BatimentService(ApplicationDbContext db) => _db = db;

        public async Task<IReadOnlyList<BatimentDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Batiments.AsNoTracking()
                .Select(b => new BatimentDto
                {
                    Id = b.Id,
                    Nom = b.Nom,
                    ResidenceId = b.ResidenceId
                })
                .ToListAsync(ct);
        }


        public async Task<IReadOnlyList<BatimentDto>> GetByResidenceAsync(Guid residenceId, CancellationToken ct = default)
        {
            return await _db.Batiments.AsNoTracking()
                .Where(b => b.ResidenceId == residenceId)
                .Select(b => new BatimentDto
                {
                    Id = b.Id,
                    Nom = b.Nom,
                    ResidenceId = b.ResidenceId,
                    NbLots = _db.Lots.Count(l => EF.Property<Guid?>(l, "BatimentId") == b.Id)
                })
                .ToListAsync(ct);
        }

        public async Task<BatimentDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var b = await _db.Batiments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (b is null) return null;

            return new BatimentDto
            {
                Id = b.Id,
                Nom = b.Nom,
                ResidenceId = b.ResidenceId,
                NbLots = await _db.Lots.CountAsync(l => EF.Property<Guid?>(l, "BatimentId") == b.Id, ct)
            };
        }

        public async Task<Guid> CreateAsync(CreateBatimentDto dto, CancellationToken ct = default)
        {
            if (!await _db.Residences.AnyAsync(r => r.Id == dto.ResidenceId, ct))
                throw new InvalidOperationException("R�sidence introuvable.");

            var entity = new Batiment { Nom = dto.Nom?.Trim() ?? string.Empty, ResidenceId = dto.ResidenceId };
            _db.Batiments.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateBatimentDto dto, CancellationToken ct = default)
        {
            var entity = await _db.Batiments.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;

            if (!await _db.Residences.AnyAsync(r => r.Id == dto.ResidenceId, ct))
                throw new InvalidOperationException("R�sidence introuvable.");

            entity.Nom = dto.Nom?.Trim() ?? entity.Nom;
            entity.ResidenceId = dto.ResidenceId;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _db.Batiments.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;

            _db.Batiments.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
