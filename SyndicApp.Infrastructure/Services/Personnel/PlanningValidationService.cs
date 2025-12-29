using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PlanningValidationService : IPlanningValidationService
    {
        private readonly ApplicationDbContext _db;

        public PlanningValidationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task ValidateAsync(ValidateMissionDto dto)
        {
            var mission = await _db.PlanningMissions
                .Include(m => m.Validation)
                .FirstOrDefaultAsync(m => m.Id == dto.MissionId)
                ?? throw new InvalidOperationException("Mission introuvable.");

            if (mission.Validation != null)
                throw new InvalidOperationException("Mission déjà validée.");

            var validation = new MissionValidation
            {
                PlanningMissionId = mission.Id,
                EstValidee = true,
                DateValidation = DateTime.UtcNow,
                Commentaire = dto.Commentaire
            };

            _db.Set<MissionValidation>().Add(validation);

            // facultatif mais propre
            mission.Validation = validation;

            await _db.SaveChangesAsync();
        }


        public async Task<IReadOnlyList<PlanningMissionDto>> GetNonValideesAsync(Guid residenceId, DateOnly date)
        {
            return await _db.PlanningMissions
                .Where(m => m.ResidenceId == residenceId
                         && m.Date == date
                         && m.Validation == null)
                .Select(m => new PlanningMissionDto(m))
                .ToListAsync();
        }
    }

}
