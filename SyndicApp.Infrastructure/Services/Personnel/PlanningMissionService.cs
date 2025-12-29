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
            // 1️⃣ Vérifier que le user existe
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == dto.EmployeId);

            if (user == null)
                throw new InvalidOperationException("Utilisateur introuvable.");

            // 2️⃣ Récupérer OU créer l’employé
            var employe = await _db.Employes
                .FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (employe == null)
            {
                employe = new Employe
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Nom = user.FullName?.Split(' ').FirstOrDefault() ?? "",
                    Prenom = user.FullName?.Split(' ').Skip(1).FirstOrDefault() ?? "",
                    Email = user.Email ?? "",
                    Poste = "Personnel"
                };

                _db.Employes.Add(employe);
                await _db.SaveChangesAsync();
            }

            // 3️⃣ Vérifier la résidence
            var residenceExists = await _db.Residences
                .AnyAsync(r => r.Id == dto.ResidenceId);

            if (!residenceExists)
                throw new InvalidOperationException("Résidence introuvable.");

            // 4️⃣ Détection conflit horaire
            var conflit = await _db.PlanningMissions.AnyAsync(p =>
                p.EmployeId == employe.Id &&
                p.Date == dto.Date &&
                dto.HeureDebut < p.HeureFin &&
                dto.HeureFin > p.HeureDebut);

            if (conflit)
                throw new InvalidOperationException("Conflit horaire détecté.");

            // 4️⃣ bis – Limite journalière (EF-safe)
            var maxConfig = await _db.ResidencePlanningConfigs
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ResidenceId == dto.ResidenceId);

            var maxHeures = maxConfig?.MaxHeuresParJour ?? 8;

            // 👉 Récupération SQL
            var missionsJour = await _db.PlanningMissions
                .Where(p => p.EmployeId == employe.Id && p.Date == dto.Date)
                .ToListAsync();

            // 👉 Calcul C#
            var heuresJour = missionsJour
                .Sum(p => (p.HeureFin - p.HeureDebut).TotalHours);

            var nouvellesHeures = (dto.HeureFin - dto.HeureDebut).TotalHours;

            if (heuresJour + nouvellesHeures > maxHeures)
            {
                throw new InvalidOperationException(
                    $"Dépassement horaire journalier ({maxHeures}h max).");
            }

            // 5️⃣ Création mission
            var entity = new PlanningMission
            {
                EmployeId = employe.Id,
                ResidenceId = dto.ResidenceId,
                Mission = dto.Mission,
                Date = dto.Date,
                HeureDebut = dto.HeureDebut,
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
