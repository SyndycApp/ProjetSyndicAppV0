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
    public class LocataireTemporaireService : ILocataireTemporaireService
    {
        private readonly ApplicationDbContext _db;
        public LocataireTemporaireService(ApplicationDbContext db) => _db = db;

        public async Task<IReadOnlyList<LocataireTemporaireDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.LocatairesTemporaires.AsNoTracking()
                .Select(l => new LocataireTemporaireDto
                {
                    Id = l.Id,
                    Nom = l.Nom,
                    Prenom = l.Prenom,
                    LotId = l.LotId,
                    DateDebut = l.DateDebut,
                    DateFin = l.DateFin
                })
                .ToListAsync(ct);
        }


        public async Task<IReadOnlyList<LocataireTemporaireDto>> GetByLotAsync(Guid lotId, CancellationToken ct = default)
        {
            return await _db.LocatairesTemporaires.AsNoTracking()
                .Where(x => x.LotId == lotId)
                .Select(x => new LocataireTemporaireDto
                {
                    Id = x.Id,
                    LotId = x.LotId,
                    Nom = x.Nom,
                    Prenom = x.Prenom,
                    Email = x.Email,
                    Telephone = x.Telephone,
                    DateDebut = x.DateDebut,
                    DateFin = x.DateFin
                }).ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(CreateLocataireTemporaireDto dto, CancellationToken ct = default)
        {
            if (!await _db.Lots.AnyAsync(l => l.Id == dto.LotId, ct))
                throw new InvalidOperationException("Lot introuvable.");

            var entity = new LocataireTemporaire
            {
                LotId = dto.LotId,
                Nom = dto.Nom?.Trim() ?? string.Empty,
                Prenom = dto.Prenom?.Trim() ?? string.Empty,
                Email = dto.Email?.Trim() ?? string.Empty,
                Telephone = dto.Telephone?.Trim() ?? string.Empty,
                DateDebut = dto.DateDebut,
                DateFin = dto.DateFin
            };

            _db.LocatairesTemporaires.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateLocataireTemporaireDto dto, CancellationToken ct = default)
        {
            var e = await _db.LocatairesTemporaires.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return false;

            e.Nom = dto.Nom?.Trim() ?? e.Nom;
            e.Prenom = dto.Prenom?.Trim() ?? e.Prenom;
            e.Email = dto.Email?.Trim() ?? e.Email;
            e.Telephone = dto.Telephone?.Trim() ?? e.Telephone;
            e.DateDebut = dto.DateDebut;
            e.DateFin = dto.DateFin;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var e = await _db.LocatairesTemporaires.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (e is null) return false;

            _db.LocatairesTemporaires.Remove(e);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
