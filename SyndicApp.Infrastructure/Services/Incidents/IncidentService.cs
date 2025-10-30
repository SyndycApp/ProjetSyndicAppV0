using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Incidents;
using SyndicApp.Application.Interfaces.Incidents;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.Incidents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Incidents
{
    public class IncidentService : IIncidentService
    {
        private readonly ApplicationDbContext _db;

        public IncidentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IncidentDto?> CreateAsync(IncidentCreateDto dto)
        {
            var entity = new Incident
            {
                Titre = dto.Titre,
                Description = dto.Description,
                TypeIncident = dto.TypeIncident,
                Urgence = dto.Urgence,
                DateDeclaration = DateTime.UtcNow,
                Statut = StatutIncident.Ouvert,
                ResidenceId = dto.ResidenceId,
                LotId = dto.LotId,
                DeclareParId = dto.DeclareParId
            };

            _db.Incidents.Add(entity);
            await _db.SaveChangesAsync();

            // historique
            _db.IncidentsHistoriques.Add(new IncidentHistorique
            {
                IncidentId = entity.Id,
                DateAction = DateTime.UtcNow,
                Action = "Création",
                AuteurId = dto.DeclareParId
            });
            await _db.SaveChangesAsync();

            return await MapToDto(entity.Id);
        }

        public async Task<IReadOnlyList<IncidentDto?>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            return await _db.Incidents.AsNoTracking()
                .OrderByDescending(i => i.DateDeclaration)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(i => new IncidentDto
                {
                    Id = i.Id,
                    Titre = i.Titre,
                    Description = i.Description,
                    TypeIncident = i.TypeIncident,
                    Urgence = i.Urgence,
                    Statut = i.Statut,
                    DateDeclaration = i.DateDeclaration,
                    ResidenceId = i.ResidenceId,
                    LotId = i.LotId,
                    DeclareParId = i.DeclareParId
                })
                .ToListAsync();
        }


        public async Task<IncidentDto?> GetByIdAsync(Guid id) => await MapToDto(id);

        public async Task<IReadOnlyList<IncidentDto?>> GetByResidenceAsync(Guid residenceId, int page = 1, int pageSize = 20)
        {
            var q = _db.Incidents.AsNoTracking()
                     .Where(i => i.ResidenceId == residenceId)
                     .OrderByDescending(i => i.DateDeclaration)
                     .Skip((page - 1) * pageSize).Take(pageSize)
                     .Select(i => new IncidentDto
                     {
                         Id = i.Id,
                         Titre = i.Titre,
                         Description = i.Description,
                         TypeIncident = i.TypeIncident,
                         Urgence = i.Urgence,
                         Statut = i.Statut,
                         DateDeclaration = i.DateDeclaration,
                         ResidenceId = i.ResidenceId,
                         LotId = i.LotId,
                         DeclareParId = i.DeclareParId
                     });

            var list = await q.ToListAsync();

            // hydrate ids liés
            var incidentIds = list.Select(x => x.Id).ToList();
            var devis = await _db.DevisTravaux.Where(d => d.IncidentId != null && incidentIds.Contains(d.IncidentId.Value))
                                              .Select(d => new { d.Id, d.IncidentId }).ToListAsync();
            var intervs = await _db.Interventions.Where(it => it.IncidentId != null && incidentIds.Contains(it.IncidentId.Value))
                                                 .Select(it => new { it.Id, it.IncidentId }).ToListAsync();

            foreach (var dto in list)
            {
                dto.DevisIds = devis.Where(d => d.IncidentId == dto.Id).Select(d => d.Id).ToList();
                dto.InterventionIds = intervs.Where(it => it.IncidentId == dto.Id).Select(it => it.Id).ToList();
            }

            return list;
        }

        public async Task<IncidentDto?> UpdateAsync(Guid id, IncidentUpdateDto dto)
        {
            var entity = await _db.Incidents.FirstOrDefaultAsync(i => i.Id == id)
                         ?? throw new InvalidOperationException("Incident introuvable.");
            if (dto.Titre != null) entity.Titre = dto.Titre;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.TypeIncident != null) entity.TypeIncident = dto.TypeIncident;
            if (dto.Urgence.HasValue) entity.Urgence = dto.Urgence.Value;
            if (dto.LotId.HasValue) entity.LotId = dto.LotId;

            await _db.SaveChangesAsync();
            return await MapToDto(id);
        }

        public async Task<IncidentDto?> ChangeStatusAsync(Guid id, IncidentChangeStatusDto dto)
        {
            var entity = await _db.Incidents.Include(i => i.Interventions).FirstOrDefaultAsync(i => i.Id == id)
                         ?? throw new InvalidOperationException("Incident introuvable.");

            // Règle : Clôture possible seulement si toutes les interventions sont Terminee
            if (dto.Statut == StatutIncident.Cloture || dto.Statut == StatutIncident.Resolue)
            {
                var notDone = entity.Interventions.Any(it => it.Statut != StatutIntervention.Terminee);
                if (notDone)
                    throw new InvalidOperationException("Impossible de clore/résoudre : des interventions ne sont pas terminées.");
            }

            var old = entity.Statut;
            entity.Statut = dto.Statut;
            await _db.SaveChangesAsync();

            _db.IncidentsHistoriques.Add(new IncidentHistorique
            {
                IncidentId = entity.Id,
                DateAction = DateTime.UtcNow,
                Action = $"Statut: {old} → {dto.Statut}",
                Commentaire = dto.Commentaire,
                AuteurId = dto.AuteurId
            });
            await _db.SaveChangesAsync();

            return await MapToDto(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Incidents.FirstOrDefaultAsync(i => i.Id == id)
                         ?? throw new InvalidOperationException("Incident introuvable.");
            if (entity.Statut == StatutIncident.Cloture)
                throw new InvalidOperationException("Suppression interdite : incident clôturé.");

            _db.Incidents.Remove(entity);
            await _db.SaveChangesAsync();
        }

        private async Task<IncidentDto?> MapToDto(Guid id)
        {
            var i = await _db.Incidents.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (i == null) return null;

            var dto = new IncidentDto
            {
                Id = i.Id,
                Titre = i.Titre,
                Description = i.Description,
                TypeIncident = i.TypeIncident,
                Urgence = i.Urgence,
                Statut = i.Statut,
                DateDeclaration = i.DateDeclaration,
                ResidenceId = i.ResidenceId,
                LotId = i.LotId,
                DeclareParId = i.DeclareParId
            };

            dto.DevisIds = await _db.DevisTravaux.Where(d => d.IncidentId == id).Select(d => d.Id).ToListAsync();
            dto.InterventionIds = await _db.Interventions.Where(it => it.IncidentId == id).Select(it => it.Id).ToListAsync();
            return dto;
        }
    }
}
