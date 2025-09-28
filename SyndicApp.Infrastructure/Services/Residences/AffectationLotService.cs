using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Application.Interfaces.Residences;
using SyndicApp.Domain.Entities.Residences;
using SyndicApp.Infrastructure.Identity; // <-- IMPORTANT : ApplicationUser
// ReSharper disable All

namespace SyndicApp.Infrastructure.Services.Residences
{
    public class AffectationLotService : IAffectationLotService
    {
        private readonly ApplicationDbContext _db;
        public AffectationLotService(ApplicationDbContext db) => _db = db;

        public async Task<IReadOnlyList<AffectationLotDto>> GetByLotAsync(Guid lotId, CancellationToken ct = default)
        {
            return await _db.AffectationsLots.AsNoTracking()
                .Where(a => a.LotId == lotId)
                .Select(a => new AffectationLotDto
                {
                    Id = a.Id,
                    LotId = a.LotId,
                    UserId = a.UserId,
                    DateDebut = a.DateDebut,
                    DateFin = a.DateFin,
                    EstProprietaire = a.EstProprietaire
                })
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<AffectationLotDto>> GetByUserAsync(Guid userId, CancellationToken ct = default)
        {
            return await _db.AffectationsLots.AsNoTracking()
                .Where(a => a.UserId == userId)
                .Select(a => new AffectationLotDto
                {
                    Id = a.Id,
                    LotId = a.LotId,
                    UserId = a.UserId,
                    DateDebut = a.DateDebut,
                    DateFin = a.DateFin,
                    EstProprietaire = a.EstProprietaire
                })
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(CreateAffectationLotDto dto, CancellationToken ct = default)
        {
            // Vérifs d’existence
            var lotExiste = await _db.Lots.AnyAsync(l => l.Id == dto.LotId, ct);
            if (!lotExiste) throw new InvalidOperationException("Lot introuvable.");

            // ⚠️ Option B : on regarde dans AspNetUsers via IdentityDbContext.Users
            var userExiste = await _db.Users.AnyAsync(u => u.Id == dto.UserId, ct);
            if (!userExiste) throw new InvalidOperationException("Utilisateur (AspNetUsers) introuvable.");

            // Anti-doublon : une affectation active (DateFin null) pour ce couple
            var hasActive = await _db.AffectationsLots
                .AnyAsync(a => a.LotId == dto.LotId && a.UserId == dto.UserId && a.DateFin == null, ct);
            if (hasActive) throw new InvalidOperationException("Une affectation active existe déjà pour ce lot/utilisateur.");

            var entity = new AffectationLot
            {
                LotId = dto.LotId,
                UserId = dto.UserId,
                DateDebut = dto.DateDebut,
                EstProprietaire = dto.EstProprietaire
            };

            _db.AffectationsLots.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity.Id;
        }

        public async Task<AffectationLotDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.AffectationsLots.AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AffectationLotDto
                {
                    Id = a.Id,
                    LotId = a.LotId,
                    UserId = a.UserId,
                    DateDebut = a.DateDebut,
                    DateFin = a.DateFin,
                    EstProprietaire = a.EstProprietaire
                })
                .SingleOrDefaultAsync(ct);
        }

        public async Task<IReadOnlyList<AffectationHistoriqueDto>> GetHistoriqueByLotAsync(Guid lotId, CancellationToken ct = default)
        {
            var query =
                from a in _db.AffectationsLots.AsNoTracking().Where(x => x.LotId == lotId)
                join u in _db.Users.AsNoTracking() on a.UserId equals u.Id into gj
                from u in gj.DefaultIfEmpty()
                orderby a.DateDebut descending
                select new AffectationHistoriqueDto
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    LotId = a.LotId,
                    EstProprietaire = a.EstProprietaire,
                    DateDebut = a.DateDebut,
                    DateFin = a.DateFin,
                    NomComplet = u != null ? u.FullName : null
                };

            return await query.ToListAsync(ct);
        }


        public async Task<IReadOnlyList<AffectationLotDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.AffectationsLots.AsNoTracking()
                .Select(a => new AffectationLotDto
                {
                    Id = a.Id,
                    LotId = a.LotId,
                    UserId = a.UserId,
                    DateDebut = a.DateDebut,
                    DateFin = a.DateFin,
                    EstProprietaire = a.EstProprietaire
                })
                .ToListAsync(ct);
        }


        public async Task<bool> CloturerAsync(Guid id, DateTime dateFin, CancellationToken ct = default)
        {
            var a = await _db.AffectationsLots.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (a is null) return false;
            if (dateFin < a.DateDebut) throw new InvalidOperationException("La date de fin ne peut pas être antérieure à la date de début.");
            a.DateFin = dateFin;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateAffectationLotDto dto, CancellationToken ct = default)
        {
            var a = await _db.AffectationsLots.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (a is null) return false;

            if (dto.DateDebut.HasValue) a.DateDebut = dto.DateDebut.Value;
            if (dto.DateFin.HasValue)
            {
                if (dto.DateFin.Value < a.DateDebut) throw new InvalidOperationException("DateFin < DateDebut.");
                a.DateFin = dto.DateFin.Value;
            }
            if (dto.EstProprietaire.HasValue) a.EstProprietaire = dto.EstProprietaire.Value;

            await _db.SaveChangesAsync(ct);
            return true;
        }


        public async Task<AffectationHistoriqueDto?> GetOccupantActuelAsync(Guid lotId, CancellationToken ct = default)
        {
            var query =
                from a in _db.AffectationsLots.AsNoTracking()
                where a.LotId == lotId && a.DateFin == null
                join u in _db.Users.AsNoTracking() on a.UserId equals u.Id into gj
                from u in gj.DefaultIfEmpty()
                select new AffectationHistoriqueDto
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    LotId = a.LotId,
                    EstProprietaire = a.EstProprietaire,
                    DateDebut = a.DateDebut,
                    DateFin = a.DateFin,
                    NomComplet = u != null ? u.FullName : null
                };

            return await query.SingleOrDefaultAsync(ct);
        }


        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _db.AffectationsLots.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;
            _db.AffectationsLots.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
