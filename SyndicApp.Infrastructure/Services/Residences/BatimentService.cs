using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Application.Interfaces.Residences;
using SyndicApp.Domain.Entities.Residences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                    ResidenceId = b.ResidenceId,
                    NbLots = _db.Lots.Count(l => EF.Property<Guid?>(l, "BatimentId") == b.Id),
                    NbEtages = b.NbEtages,
                    Bloc = b.Bloc,
                    ResponsableNom = b.ResponsableNom,
                    HasAscenseur = b.HasAscenseur,
                    AnneeConstruction = b.AnneeConstruction,
                    CodeAcces = b.CodeAcces
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
                    NbLots = _db.Lots.Count(l => EF.Property<Guid?>(l, "BatimentId") == b.Id),
                    NbEtages = b.NbEtages,
                    Bloc = b.Bloc,
                    ResponsableNom = b.ResponsableNom,
                    HasAscenseur = b.HasAscenseur,
                    AnneeConstruction = b.AnneeConstruction,
                    CodeAcces = b.CodeAcces
                })
                .ToListAsync(ct);
        }

        public async Task<BatimentDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var b = await _db.Batiments.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (b is null) return null;

            return new BatimentDto
            {
                Id = b.Id,
                Nom = b.Nom,
                ResidenceId = b.ResidenceId,
                NbLots = await _db.Lots.CountAsync(l => EF.Property<Guid?>(l, "BatimentId") == b.Id, ct),
                NbEtages = b.NbEtages,
                Bloc = b.Bloc,
                ResponsableNom = b.ResponsableNom,
                HasAscenseur = b.HasAscenseur,
                AnneeConstruction = b.AnneeConstruction,
                CodeAcces = b.CodeAcces
            };
        }

        public async Task<Guid> CreateAsync(CreateBatimentDto dto, CancellationToken ct = default)
        {
            if (!await _db.Residences.AnyAsync(r => r.Id == dto.ResidenceId, ct))
                throw new InvalidOperationException("Résidence introuvable.");

            var entity = new Batiment
            {
                Nom = dto.Nom?.Trim() ?? string.Empty,
                ResidenceId = dto.ResidenceId,
                NbEtages = dto.NbEtages,
                Bloc = dto.Bloc,
                ResponsableNom = dto.ResponsableNom,
                HasAscenseur = dto.HasAscenseur,
                AnneeConstruction = dto.AnneeConstruction,
                CodeAcces = dto.CodeAcces
            };

            _db.Batiments.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateBatimentDto dto, CancellationToken ct = default)
        {
            var entity = await _db.Batiments.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;

            if (!await _db.Residences.AnyAsync(r => r.Id == dto.ResidenceId, ct))
                throw new InvalidOperationException("Résidence introuvable.");

            entity.Nom = dto.Nom?.Trim() ?? entity.Nom;
            entity.ResidenceId = dto.ResidenceId;
            entity.NbEtages = dto.NbEtages;
            entity.Bloc = dto.Bloc;
            entity.ResponsableNom = dto.ResponsableNom;
            entity.HasAscenseur = dto.HasAscenseur;
            entity.AnneeConstruction = dto.AnneeConstruction;
            entity.CodeAcces = dto.CodeAcces;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IReadOnlyList<BatimentDto>> GetForUserAsync(Guid userId, CancellationToken ct = default)
        {
            var query =
                from b in _db.Batiments.AsNoTracking()
                join l in _db.Lots.AsNoTracking()
                    on b.Id equals EF.Property<Guid?>(l, "BatimentId")
                join a in _db.AffectationsLots.AsNoTracking()
                        .Where(x => x.DateFin == null)
                    on l.Id equals a.LotId
                where a.UserId == userId
                group new { b, l } by new
                {
                    b.Id,
                    b.Nom,
                    b.ResidenceId,
                    b.NbEtages,
                    b.Bloc,
                    b.ResponsableNom,
                    b.HasAscenseur,
                    b.AnneeConstruction,
                    b.CodeAcces
                }
                into g
                select new BatimentDto
                {
                    Id = g.Key.Id,
                    Nom = g.Key.Nom,
                    ResidenceId = g.Key.ResidenceId,
                    NbLots = g.Count(),
                    NbEtages = g.Key.NbEtages,
                    Bloc = g.Key.Bloc,
                    ResponsableNom = g.Key.ResponsableNom,
                    HasAscenseur = g.Key.HasAscenseur,
                    AnneeConstruction = g.Key.AnneeConstruction,
                    CodeAcces = g.Key.CodeAcces
                };

            return await query.ToListAsync(ct);
        }

        public async Task<Guid?> ResolveIdByNameAsync(string nom, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(nom)) return null;

            var normalized = nom.Trim();

            var matches = await _db.Batiments.AsNoTracking()
                .Where(b => b.Nom == normalized)
                .Select(b => b.Id)
                .Take(2)
                .ToListAsync(ct);

            if (matches.Count == 1) return matches[0];
            if (matches.Count > 1) return null;

            return null;
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
