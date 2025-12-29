using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Application.DTOs.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PlanningMissionService : IPlanningMissionService
    {
        private readonly ApplicationDbContext _db;

        public PlanningMissionService(ApplicationDbContext db)
        {
            _db = db;
        }

        // ================= CREATE =================
        public async Task<Guid> CreateAsync(CreatePlanningMissionDto dto)
        {
            // 1️⃣ Vérifier que l'employé existe (TABLE Employes)
            var employe = await _db.Employes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == dto.EmployeId);

            if (employe == null)
                throw new InvalidOperationException("Employé introuvable.");

            // 2️⃣ Vérifier la résidence
            var residenceExists = await _db.Residences
                .AnyAsync(r => r.Id == dto.ResidenceId);

            if (!residenceExists)
                throw new InvalidOperationException("Résidence introuvable.");

            // 3️⃣ Normalisation heures (MISSION DE NUIT)
            var heureDebut = dto.HeureDebut;
            var heureFin = dto.HeureFin;

            if (heureFin <= heureDebut)
                heureFin = heureFin.Add(TimeSpan.FromDays(1));

            // 4️⃣ Récupérer missions existantes du jour
            var missionsJour = await _db.PlanningMissions
                .Where(p =>
                    p.EmployeId == dto.EmployeId &&
                    p.Date == dto.Date)
                .ToListAsync();

            // 5️⃣ Détection conflits horaires
            foreach (var p in missionsJour)
            {
                var pDebut = p.HeureDebut;
                var pFin = p.HeureFin;

                if (pFin <= pDebut)
                    pFin = pFin.Add(TimeSpan.FromDays(1));

                if (heureDebut < pFin && heureFin > pDebut)
                    throw new InvalidOperationException("Conflit horaire détecté.");
            }

            // 6️⃣ Limite journalière par résidence
            var maxConfig = await _db.ResidencePlanningConfigs
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ResidenceId == dto.ResidenceId);

            var maxHeures = maxConfig?.MaxHeuresParJour ?? 8;

            var heuresJour = missionsJour.Sum(p =>
            {
                var d = p.HeureDebut;
                var f = p.HeureFin;

                if (f <= d)
                    f = f.Add(TimeSpan.FromDays(1));

                return (f - d).TotalHours;
            });

            var nouvellesHeures = (heureFin - heureDebut).TotalHours;

            if (heuresJour + nouvellesHeures > maxHeures)
                throw new InvalidOperationException(
                    $"Dépassement horaire journalier ({maxHeures}h max).");

            // 7️⃣ Création mission
            var entity = new PlanningMission
            {
                EmployeId = dto.EmployeId,
                ResidenceId = dto.ResidenceId,
                Mission = dto.Mission,
                Date = dto.Date,
                HeureDebut = dto.HeureDebut, // stock brut
                HeureFin = dto.HeureFin,
                Statut = "Planifiee"
            };

            _db.PlanningMissions.Add(entity);
            await _db.SaveChangesAsync();

            return entity.Id;
        }

        // ================= UPDATE =================
        public async Task UpdateAsync(Guid id, UpdatePlanningMissionDto dto)
        {
            var entity = await _db.PlanningMissions.FindAsync(id)
                ?? throw new InvalidOperationException("Mission introuvable.");

            // 🔥 Vérification conflit (hors mission courante)
            var conflit = await _db.PlanningMissions.AnyAsync(p =>
                p.Id != id &&
                p.EmployeId == entity.EmployeId &&
                p.Date == entity.Date &&
                dto.HeureDebut < p.HeureFin &&
                dto.HeureFin > p.HeureDebut);

            if (conflit)
                throw new InvalidOperationException("Conflit horaire détecté.");

            entity.Mission = dto.Mission;
            entity.HeureDebut = dto.HeureDebut;
            entity.HeureFin = dto.HeureFin;
            entity.Statut = dto.Statut;

            await _db.SaveChangesAsync();
        }

        // ================= DELETE =================
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.PlanningMissions.FindAsync(id);
            if (entity == null)
                return;

            _db.PlanningMissions.Remove(entity);
            await _db.SaveChangesAsync();
        }

        // ================= GET BY EMPLOYE =================
        public async Task<IReadOnlyList<PlanningMissionDto>> GetByEmployeAsync(Guid employeId)
        {
            // 🔁 Mapper UserId → Employe
            var employe = await _db.Employes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UserId == employeId);

            if (employe == null)
                return Array.Empty<PlanningMissionDto>();

            return await _db.PlanningMissions
                .Where(p => p.EmployeId == employe.Id)
                .OrderBy(p => p.Date)
                .ThenBy(p => p.HeureDebut)
                .Select(p => new PlanningMissionDto(p))
                .ToListAsync();
        }

        // ================= GET BY RESIDENCE =================
        public async Task<IReadOnlyList<PlanningMissionDto>> GetByResidenceAsync(Guid residenceId)
        {
            return await _db.PlanningMissions
                .Where(p => p.ResidenceId == residenceId)
                .OrderBy(p => p.Date)
                .ThenBy(p => p.HeureDebut)
                .Select(p => new PlanningMissionDto(p))
                .ToListAsync();
        }
    }
}
