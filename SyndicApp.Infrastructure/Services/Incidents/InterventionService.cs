using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Interventions;
using SyndicApp.Application.Interfaces.Incidents;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.Incidents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Incidents
{
    public class InterventionService : IInterventionService
    {
        private readonly ApplicationDbContext _db;

        public InterventionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<InterventionDto?> CreateAsync(InterventionCreateDto dto)
        {
            var entity = new Intervention
            {
                Description = dto.Description,
                ResidenceId = dto.ResidenceId,
                DevisTravauxId = dto.DevisTravauxId,
                IncidentId = dto.IncidentId,
                EmployeId = dto.EmployeId,
                PrestataireExterne = dto.PrestataireExterne,
                DatePrevue = dto.DatePrevue,
                CoutEstime = dto.CoutEstime,
                Statut = StatutIntervention.Planifiee
            };

            _db.Interventions.Add(entity);
            await _db.SaveChangesAsync();

            _db.InterventionsHistoriques.Add(new InterventionHistorique
            {
                InterventionId = entity.Id,
                DateAction = DateTime.UtcNow,
                Action = "Création",
                AuteurId = Guid.Empty
            });
            await _db.SaveChangesAsync();

            return await Map(entity.Id);
        }

        public async Task<InterventionDto?> GetByIdAsync(Guid id) => await Map(id);

        public async Task<IReadOnlyList<InterventionDto?>> GetByResidenceAsync(Guid residenceId, int page = 1, int pageSize = 20)
        {
            return await _db.Interventions.AsNoTracking()
                .Where(i => i.ResidenceId == residenceId)
                .OrderByDescending(i => i.DatePrevue ?? i.DateRealisation ?? DateTime.MinValue)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(i => new InterventionDto
                {
                    Id = i.Id,
                    Description = i.Description,
                    ResidenceId = i.ResidenceId,
                    DevisTravauxId = i.DevisTravauxId,
                    IncidentId = i.IncidentId,
                    EmployeId = i.EmployeId,
                    PrestataireExterne = i.PrestataireExterne,
                    DatePrevue = i.DatePrevue,
                    DateRealisation = i.DateRealisation,
                    CoutEstime = i.CoutEstime,
                    CoutReel = i.CoutReel,
                    Statut = i.Statut
                }).ToListAsync();
        }

        public async Task<IReadOnlyList<InterventionDto?>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            return await _db.Interventions.AsNoTracking()
                .OrderByDescending(i => i.DatePrevue ?? i.DateRealisation ?? DateTime.MinValue)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(i => new InterventionDto
                {
                    Id = i.Id,
                    Description = i.Description,
                    ResidenceId = i.ResidenceId,
                    DevisTravauxId = i.DevisTravauxId,
                    IncidentId = i.IncidentId,
                    EmployeId = i.EmployeId,
                    PrestataireExterne = i.PrestataireExterne,
                    DatePrevue = i.DatePrevue,
                    DateRealisation = i.DateRealisation,
                    CoutEstime = i.CoutEstime,
                    CoutReel = i.CoutReel,
                    Statut = i.Statut
                })
                .ToListAsync();
        }


        public async Task<InterventionDto?> ChangeStatusAsync(Guid id, InterventionChangeStatusDto dto)
        {
            var entity = await _db.Interventions.FirstOrDefaultAsync(i => i.Id == id)
                         ?? throw new InvalidOperationException("Intervention introuvable.");

            var old = entity.Statut;
            entity.Statut = dto.Statut;

            if (dto.Statut == StatutIntervention.Terminee)
            {
                entity.DateRealisation = dto.DateRealisation ?? DateTime.UtcNow;
                entity.CoutReel = dto.CoutReel;
            }

            await _db.SaveChangesAsync();

            _db.InterventionsHistoriques.Add(new InterventionHistorique
            {
                InterventionId = entity.Id,
                DateAction = DateTime.UtcNow,
                Action = $"Statut: {old} → {dto.Statut}",
                Commentaire = dto.Commentaire,
                AuteurId = dto.AuteurId
            });
            await _db.SaveChangesAsync();

            return await Map(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Interventions.FirstOrDefaultAsync(i => i.Id == id)
                         ?? throw new InvalidOperationException("Intervention introuvable.");
            _db.Interventions.Remove(entity);
            await _db.SaveChangesAsync();
        }

        private async Task<InterventionDto?> Map(Guid id)
        {
            var i = await _db.Interventions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (i == null) return null;

            return new InterventionDto
            {
                Id = i.Id,
                Description = i.Description,
                ResidenceId = i.ResidenceId,
                DevisTravauxId = i.DevisTravauxId,
                IncidentId = i.IncidentId,
                EmployeId = i.EmployeId,
                PrestataireExterne = i.PrestataireExterne,
                DatePrevue = i.DatePrevue,
                DateRealisation = i.DateRealisation,
                CoutEstime = i.CoutEstime,
                CoutReel = i.CoutReel,
                Statut = i.Statut
            };
        }
    }
}
