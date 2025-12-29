using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Application.Interfaces.Common;
using SyndicApp.Application.Interfaces.Residences;
using SyndicApp.Domain.Entities.Residences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Residences
{
    public class ResidenceService : IResidenceService
    {
        private readonly ApplicationDbContext _db;
        private readonly IGeocodingService _geo;

        public ResidenceService(ApplicationDbContext db, IGeocodingService geo)
        {
            _db = db;
            _geo = geo;
        }

        public async Task<IReadOnlyList<ResidenceDto>> GetAllAsync(CancellationToken ct = default)
        {
            // Projection directe avec comptages dérivés
            return await _db.Residences
                .AsNoTracking()
                .Select(r => new ResidenceDto
                {
                    Id = r.Id,
                    Nom = r.Nom,
                    Adresse = r.Adresse,
                    Ville = r.Ville,
                    CodePostal = r.CodePostal,
                    NbBatiments = _db.Batiments.Count(b => b.ResidenceId == r.Id),
                    NbLots = _db.Lots.Count(l => l.ResidenceId == r.Id),

                    NbIncidents = _db.Incidents.Count(i => _db.Lots.Any(l => l.Id == i.LotId && l.ResidenceId == r.Id))
                })
                .ToListAsync(ct);
        }

        public async Task<ResidenceDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var dto = await _db.Residences
                .AsNoTracking()
                .Where(r => r.Id == id)
                .Select(r => new ResidenceDto
                {
                    Id = r.Id,
                    Nom = r.Nom,
                    Adresse = r.Adresse,
                    Ville = r.Ville,
                    CodePostal = r.CodePostal,

                    // Sous-requêtes traduites côté SQL → évite toute valeur nullable
                    NbBatiments = _db.Batiments.Count(b => b.ResidenceId == r.Id),
                    NbLots = _db.Lots.Count(l => l.ResidenceId == r.Id),
                    NbIncidents = (
                        from i in _db.Incidents
                        join l in _db.Lots on i.LotId equals l.Id
                        where l.ResidenceId == r.Id
                        select i.Id
                    ).Count()
                })
                .SingleOrDefaultAsync(ct);

            return dto; // null => NotFound dans le controller
        }

        public async Task<Guid?> LookupIdByNameAsync(string name, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var normalized = name.Trim().ToLower();

            var id = await _db.Residences
                .AsNoTracking()
                .Where(r => r.Nom.ToLower() == normalized)
                .Select(r => (Guid?)r.Id)
                .FirstOrDefaultAsync(ct);

            return id; // null si introuvable
        }
        public async Task<Guid> CreateAsync(CreateResidenceDto dto, CancellationToken ct = default)
        {
            double latitude = dto.Latitude ?? 0;
            double longitude = dto.Longitude ?? 0;

            // 🔥 AUTO-GEO SI ABSENT
            if (latitude == 0 && longitude == 0)
            {
                var address =
                    $"{dto.Adresse}, {dto.CodePostal}, {dto.Ville}, Maroc";

                var geo = await _geo.GeocodeAsync(address);
                if (geo.HasValue)
                {
                    latitude = geo.Value.lat;
                    longitude = geo.Value.lng;
                }
            }

            var entity = new Residence
            {
                Nom = dto.Nom.Trim(),
                Adresse = dto.Adresse.Trim(),
                Ville = dto.Ville.Trim(),
                CodePostal = dto.CodePostal.Trim(),

                Latitude = latitude,
                Longitude = longitude,
                RayonAutoriseMetres = dto.RayonAutoriseMetres ?? 100
            };

            _db.Residences.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity.Id;
        }


        public async Task<bool> UpdateAsync(Guid id, UpdateResidenceDto dto, CancellationToken ct = default)
        {
            var entity = await _db.Residences.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;

            entity.Nom = dto.Nom?.Trim() ?? entity.Nom;
            entity.Adresse = dto.Adresse?.Trim() ?? entity.Adresse;
            entity.Ville = dto.Ville?.Trim() ?? entity.Ville;
            entity.CodePostal = dto.CodePostal?.Trim() ?? entity.CodePostal;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _db.Residences.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;

            _db.Residences.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IReadOnlyList<LotDto>> GetLotsAsync(Guid residenceId, CancellationToken ct = default)
        {
            return await _db.Lots
                .AsNoTracking()
                .Where(l => l.ResidenceId == residenceId)
                .Select(l => new LotDto
                {
                    Id = l.Id,
                    NumeroLot = l.NumeroLot,
                    Type = l.Type,
                    Surface = l.Surface,
                    ResidenceId = l.ResidenceId,
                    // BatimentId est un shadow property (nullable)
                    BatimentId = EF.Property<Guid?>(l, "BatimentId")
                })
                .ToListAsync(ct);
        }


        private async Task<List<LotWithOccupantDto>> GetLotsByResidenceAsync(Guid residenceId, CancellationToken ct)
        {
            var query =
                from l in _db.Lots.AsNoTracking()
                where l.ResidenceId == residenceId
                join a in _db.AffectationsLots.AsNoTracking().Where(x => x.DateFin == null)
                    on l.Id equals a.LotId into gj
                from a in gj.DefaultIfEmpty()
                select new LotWithOccupantDto
                {
                    // Lot
                    LotId = l.Id,
                    NumeroLot = l.NumeroLot,
                    Type = l.Type,
                    Surface = l.Surface,
                    ResidenceId = l.ResidenceId,
                    // BatimentId est une shadow property sur Lot
                    BatimentId = EF.Property<Guid?>(l, "BatimentId"),

                    // Occupant (peut être null si aucun actif)
                    OccupantUserId = a != null ? a.UserId : (Guid?)null,
                    EstProprietaire = a != null ? a.EstProprietaire : (bool?)null,
                    DateDebut = a != null ? a.DateDebut : (DateTime?)null,
                    DateFin = a != null ? a.DateFin : (DateTime?)null
                };

            return await query.ToListAsync(ct);
        }



        public async Task<ResidenceDetailsDto?> GetResidenceDetailsAsync(Guid residenceId, CancellationToken ct = default)
        {
            var res = await _db.Residences.AsNoTracking()
                .Where(r => r.Id == residenceId)
                .Select(r => new ResidenceDetailsDto
                {
                    ResidenceId = r.Id,
                    Nom = r.Nom,
                    Adresse = r.Adresse,
                    Ville = r.Ville,
                    CodePostal = r.CodePostal,
                    NbLots = _db.Lots.Count(l => l.ResidenceId == r.Id),
                    NbOccupantsActifs = _db.AffectationsLots.Count(a => _db.Lots
                        .Any(l => l.Id == a.LotId && l.ResidenceId == r.Id) && a.DateFin == null),
                    Lots = new List<LotWithOccupantDto>() // rempli ensuite
                })
                .FirstOrDefaultAsync(ct);

            if (res is null) return null;
            res.Lots = await GetLotsByResidenceAsync(residenceId, ct);
            return res;
        }

        public async Task<IReadOnlyList<ResidenceDto>> GetForUserAsync(Guid userId, CancellationToken ct = default)
        {
            var query =
                from r in _db.Residences.AsNoTracking()
                join l in _db.Lots.AsNoTracking() on r.Id equals l.ResidenceId
                join a in _db.AffectationsLots.AsNoTracking().Where(x => x.DateFin == null)
                    on l.Id equals a.LotId
                where a.UserId == userId
                group new { r, l, a } by new { r.Id, r.Nom, r.Adresse, r.Ville, r.CodePostal } into g
                select new ResidenceDto
                {
                    Id = g.Key.Id,
                    Nom = g.Key.Nom,
                    Adresse = g.Key.Adresse,
                    Ville = g.Key.Ville,
                    CodePostal = g.Key.CodePostal,
                    NbBatiments = _db.Batiments.Count(b => b.ResidenceId == g.Key.Id),
                    NbLots = _db.Lots.Count(l => l.ResidenceId == g.Key.Id),
                    NbIncidents = (
                        from i in _db.Incidents
                        join l in _db.Lots on i.LotId equals l.Id
                        where l.ResidenceId == g.Key.Id
                        select i.Id
                    ).Count()
                };

            return await query.ToListAsync(ct);
        }

        public async Task<IReadOnlyList<ResidenceOccupantDto>> GetOccupantsAsync(Guid residenceId, CancellationToken ct = default)
        {
            var query =
                from l in _db.Lots.AsNoTracking()
                where l.ResidenceId == residenceId
                join a in _db.AffectationsLots.AsNoTracking().Where(x => x.DateFin == null)
                    on l.Id equals a.LotId into gj
                from a in gj.DefaultIfEmpty()
                select new ResidenceOccupantDto
                {
                    LotId = l.Id,
                    NumeroLot = l.NumeroLot,

                    UserId = a != null ? a.UserId : (Guid?)null,

                    // Option B : lecture dans AspNetUsers
                    NomComplet = a != null
                        ? _db.Users
                            .Where(u => u.Id == a.UserId)
                            .Select(u => ((string)((u.FullName ?? u.Email) ?? "")))
                            .FirstOrDefault()
                        : null,

                    EstProprietaire = a == null ? (bool?)null : a.EstProprietaire,
                    DateDebut = a == null ? (DateTime?)null : a.DateDebut,
                    DateFin = a == null ? (DateTime?)null : a.DateFin
                };

            return await query.ToListAsync(ct);
        }


    }
}
