using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Devis;
using SyndicApp.Application.Interfaces.Incidents;
using SyndicApp.Domain.Entities.Incidents.Enums;
using SyndicApp.Domain.Entities.Incidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Incidents
{
    public class DevisTravauxService : IDevisTravauxService
    {
        private readonly ApplicationDbContext _db;

        public DevisTravauxService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<DevisDto?> CreateAsync(DevisCreateDto dto)
        {
            var entity = new DevisTravaux
            {
                Titre = dto.Titre,
                Description = dto.Description,
                DateEmission = DateTime.UtcNow,
                Statut = StatutDevis.EnAttente,
                MontantHT = dto.MontantHT,
                TauxTVA = dto.TauxTVA,
                ResidenceId = dto.ResidenceId,
                IncidentId = dto.IncidentId
            };

            _db.DevisTravaux.Add(entity);
            await _db.SaveChangesAsync();

            _db.DevisHistoriques.Add(new DevisHistorique
            {
                DevisTravauxId = entity.Id,
                DateAction = DateTime.UtcNow,
                Action = "Création",
                AuteurId = Guid.Empty
            });
            await _db.SaveChangesAsync();

            return await Map(entity.Id);
        }

        public async Task<DevisDto?> ResolveByTitleAsync(string titre)
        {
            if (string.IsNullOrWhiteSpace(titre))
                return null;

            var devis = await _db.DevisTravaux
                .AsNoTracking()
                .Where(d => EF.Functions.Like(d.Titre, $"%{titre}%"))
                .OrderByDescending(d => d.DateEmission)
                .FirstOrDefaultAsync();

            if (devis == null)
                return null;

            return await MapToDto(devis.Id);
        }
        public async Task<IReadOnlyList<DevisDto?>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            var list = await _db.DevisTravaux.AsNoTracking()
                .OrderByDescending(d => d.DateEmission)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(d => new DevisDto
                {
                    Id = d.Id,
                    Titre = d.Titre,
                    Description = d.Description,
                    MontantHT = d.MontantHT,
                    TauxTVA = d.TauxTVA,
                    MontantTTC = Math.Round(d.MontantHT * (1 + d.TauxTVA), 2),
                    ResidenceId = d.ResidenceId,
                    IncidentId = d.IncidentId,
                    Statut = d.Statut,
                    DateEmission = d.DateEmission,
                    ValideParId = d.ValideParId,
                    DateDecision = d.DateDecision,
                    CommentaireDecision = d.CommentaireDecision
                })
                .ToListAsync();

            var ids = list.Select(x => x.Id).ToList();
            var intervs = await _db.Interventions
                .Where(i => i.DevisTravauxId != null && ids.Contains(i.DevisTravauxId!.Value))
                .Select(i => new { i.Id, i.DevisTravauxId })
                .ToListAsync();

            foreach (var d in list)
                d.InterventionIds = intervs.Where(i => i.DevisTravauxId == d.Id).Select(i => i.Id).ToList();

            return list;
        }


        public async Task<DevisDto?> GetByIdAsync(Guid id) => await Map(id);

        public async Task<IReadOnlyList<DevisDto?>> GetByResidenceAsync(Guid residenceId, int page = 1, int pageSize = 20)
        {
            var list = await _db.DevisTravaux.AsNoTracking()
                .Where(d => d.ResidenceId == residenceId)
                .OrderByDescending(d => d.DateEmission)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(d => new DevisDto
                {
                    Id = d.Id,
                    Titre = d.Titre,
                    Description = d.Description,
                    MontantHT = d.MontantHT,
                    TauxTVA = d.TauxTVA,
                    MontantTTC = Math.Round(d.MontantHT * (1 + d.TauxTVA), 2),
                    ResidenceId = d.ResidenceId,
                    IncidentId = d.IncidentId,
                    Statut = d.Statut,
                    DateEmission = d.DateEmission,
                    ValideParId = d.ValideParId,
                    DateDecision = d.DateDecision,
                    CommentaireDecision = d.CommentaireDecision
                }).ToListAsync();

            var ids = list.Select(x => x.Id).ToList();
            var intervs = await _db.Interventions.Where(i => i.DevisTravauxId != null && ids.Contains(i.DevisTravauxId.Value))
                                                 .Select(i => new { i.Id, i.DevisTravauxId }).ToListAsync();
            foreach (var d in list)
                d.InterventionIds = intervs.Where(i => i.DevisTravauxId == d.Id).Select(i => i.Id).ToList();

            return list;
        }

        public async Task<DevisDto?> DecideAsync(Guid devisId, DevisDecisionDto dto)
        {
            var devis = await _db.DevisTravaux.FirstOrDefaultAsync(d => d.Id == devisId)
                        ?? throw new InvalidOperationException("Devis introuvable.");

            var old = devis.Statut;
            devis.Statut = dto.Statut;
            devis.ValideParId = dto.AuteurId;
            devis.DateDecision = dto.DateDecision ?? DateTime.UtcNow;
            devis.CommentaireDecision = dto.Commentaire;
            await _db.SaveChangesAsync();

            _db.DevisHistoriques.Add(new DevisHistorique
            {
                DevisTravauxId = devis.Id,
                DateAction = DateTime.UtcNow,
                Action = $"Statut: {old} → {dto.Statut}",
                Commentaire = dto.Commentaire,
                AuteurId = dto.AuteurId
            });
            await _db.SaveChangesAsync();

            // Règle: si accepté → créer intervention Planifiée (si pas déjà faite)
            if (dto.Statut == StatutDevis.Accepte)
            {
                var exists = await _db.Interventions.AnyAsync(i => i.DevisTravauxId == devis.Id);
                if (!exists)
                {
                    _db.Interventions.Add(new Domain.Entities.Incidents.Intervention
                    {
                        Description = $"Intervention suite au devis #{devis.Id} - {devis.Titre}",
                        Statut = StatutIntervention.Planifiee,
                        ResidenceId = devis.ResidenceId,
                        DevisTravauxId = devis.Id,
                        IncidentId = devis.IncidentId,
                        DatePrevue = DateTime.UtcNow.AddDays(3) // par défaut
                    });
                    await _db.SaveChangesAsync();
                }
            }

            return await Map(devis.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var devis = await _db.DevisTravaux.FirstOrDefaultAsync(d => d.Id == id)
                        ?? throw new InvalidOperationException("Devis introuvable.");
            if (devis.Statut == StatutDevis.Termine)
                throw new InvalidOperationException("Suppression interdite : devis terminé.");

            _db.DevisTravaux.Remove(devis);
            await _db.SaveChangesAsync();
        }

        private async Task<DevisDto?> Map(Guid id)
        {
            var d = await _db.DevisTravaux.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (d == null) return null;

            var dto = new DevisDto
            {
                Id = d.Id,
                Titre = d.Titre,
                Description = d.Description,
                MontantHT = d.MontantHT,
                TauxTVA = d.TauxTVA,
                MontantTTC = Math.Round(d.MontantHT * (1 + d.TauxTVA), 2),
                ResidenceId = d.ResidenceId,
                IncidentId = d.IncidentId,
                Statut = d.Statut,
                DateEmission = d.DateEmission,
                ValideParId = d.ValideParId,
                DateDecision = d.DateDecision,
                CommentaireDecision = d.CommentaireDecision
            };

            dto.InterventionIds = await _db.Interventions.Where(i => i.DevisTravauxId == id).Select(i => i.Id).ToListAsync();
            return dto;
        }

        // ============= MAPPING PRIVÉ =============
        private async Task<DevisDto?> MapToDto(Guid id)
        {
            var d = await _db.DevisTravaux
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (d == null)
                return null;

            return new DevisDto
            {
                Id = d.Id,
                Titre = d.Titre,
                Description = d.Description,
                MontantHT = d.MontantHT,
                TauxTVA = d.TauxTVA,
                ResidenceId = d.ResidenceId,
                IncidentId = d.IncidentId,
                Statut = d.Statut,
                DateEmission = d.DateEmission,
                ValideParId = d.ValideParId,
                DateDecision = d.DateDecision
            };
        }
    }
}
