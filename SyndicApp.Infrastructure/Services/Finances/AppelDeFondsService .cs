// SyndicApp.Infrastructure/Services/Finances/AppelDeFondsService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Finances;
using SyndicApp.Application.Interfaces.Finances;

namespace SyndicApp.Infrastructure.Services.Finances
{
    public class AppelDeFondsService : IAppelDeFondsService
    {
        private readonly ApplicationDbContext _db;

        public AppelDeFondsService(ApplicationDbContext db) => _db = db;

        public async Task<Guid?> ResolveIdByDescriptionAsync(string description, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(description))
                return null;

            var normalized = description.Trim().ToLower();

            var appel = await _db.AppelsDeFonds
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Description.ToLower() == normalized, ct);

            return appel?.Id;
        }

        public async Task<string?> GetDescriptionByIdAsync(Guid id, CancellationToken ct = default)
        {
            var description = await _db.AppelsDeFonds
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => a.Description)
                .FirstOrDefaultAsync(ct);

            return string.IsNullOrEmpty(description) ? null : description;
        }


        public async Task<IReadOnlyList<AppelDeFondsDto>> GetAllAsync(CancellationToken ct = default)
        {
            // ⚠️ On garde la même logique qu’avant, on ajoute juste ResidenceNom
            return await _db.AppelsDeFonds
                .AsNoTracking()
                .Select(a => new AppelDeFondsDto
                {
                    Id = a.Id,
                    Description = a.Description,
                    MontantTotal = a.MontantTotal,
                    DateEmission = a.DateEmission,
                    ResidenceId = a.ResidenceId,

                    // ✅ nouveau : nom lisible de la résidence
                    ResidenceNom = a.Residence != null ? a.Residence.Nom : string.Empty,

                    NbPaiements = _db.Paiements.Count(p => p.AppelDeFondsId == a.Id),
                    MontantPaye = _db.Paiements
                        .Where(p => p.AppelDeFondsId == a.Id)
                        .Sum(p => (decimal?)p.Montant) ?? 0m
                    // Paiements : laissé vide pour la liste (DTO initialise déjà à new())
                })
                .ToListAsync(ct);
        }

        public async Task<AppelDeFondsDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            // ⚠️ On garde la logique, on enrichit juste avec ResidenceNom + liste des paiements
            return await _db.AppelsDeFonds
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AppelDeFondsDto
                {
                    Id = a.Id,
                    Description = a.Description,
                    MontantTotal = a.MontantTotal,
                    DateEmission = a.DateEmission,
                    ResidenceId = a.ResidenceId,

                    // ✅ nom de la résidence
                    ResidenceNom = a.Residence != null ? a.Residence.Nom : string.Empty,

                    NbPaiements = _db.Paiements.Count(p => p.AppelDeFondsId == a.Id),
                    MontantPaye = _db.Paiements
                        .Where(p => p.AppelDeFondsId == a.Id)
                        .Sum(p => (decimal?)p.Montant) ?? 0m,

                    // ✅ remplissage de la liste des paiements pour la page Détails
                    Paiements = a.Paiements
                        .OrderByDescending(p => p.DatePaiement)
                        .Select(p => new PaiementDto
                        {
                            Id = p.Id,
                            Montant = p.Montant,
                            DatePaiement = p.DatePaiement,
                        })
                        .ToList()
                })
                .SingleOrDefaultAsync(ct);
        }

        public async Task<Guid> CreateAsync(CreateAppelDeFondsDto dto, CancellationToken ct = default)
        {
            if (dto.MontantTotal < 0)
                throw new InvalidOperationException("MontantTotal doit être ≥ 0.");

            var entity = new Domain.Entities.Finances.AppelDeFonds
            {
                Description = dto.Description?.Trim() ?? string.Empty,
                MontantTotal = dto.MontantTotal,
                DateEmission = dto.DateEmission,
                ResidenceId = dto.ResidenceId
            };

            _db.AppelsDeFonds.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateAppelDeFondsDto dto, CancellationToken ct = default)
        {
            var a = await _db.AppelsDeFonds.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (a is null)
                return false;

            // Interdit si clôturé (on garde exactement cette règle)
            var estCloture = _db.Entry(a).Property<bool>("EstCloture").CurrentValue;
            if (estCloture)
                throw new InvalidOperationException("Appel clôturé — modification interdite.");

            if (dto.MontantTotal < 0)
                throw new InvalidOperationException("MontantTotal doit être ≥ 0.");

            a.Description = dto.Description?.Trim() ?? a.Description;
            a.MontantTotal = dto.MontantTotal;
            a.DateEmission = dto.DateEmission;
            a.ResidenceId = dto.ResidenceId; // ✅ on garde le fait qu’on puisse changer la résidence

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> CloturerAsync(Guid id, CancellationToken ct = default)
        {
            var a = await _db.AppelsDeFonds.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (a is null)
                return false;

            _db.Entry(a).Property<bool>("EstCloture").CurrentValue = true;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var a = await _db.AppelsDeFonds.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (a is null)
                return false;

            // Interdire si des paiements existent (règle métier conservée)
            var hasPayments = await _db.Paiements.AnyAsync(p => p.AppelDeFondsId == id, ct);
            if (hasPayments)
                throw new InvalidOperationException("Impossible de supprimer : des paiements existent.");

            _db.AppelsDeFonds.Remove(a);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
