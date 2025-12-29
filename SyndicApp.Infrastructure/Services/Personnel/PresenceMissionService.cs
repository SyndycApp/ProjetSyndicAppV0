using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Domain.Entities.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel;

public class PresenceMissionService : IPresenceMissionService
{
    private readonly ApplicationDbContext _db;
    private readonly IGeoPresenceService _geo;

    public PresenceMissionService(ApplicationDbContext db, IGeoPresenceService geo)
    {
        _db = db;
        _geo = geo;
    }

    public async Task StartAsync(Guid userId, StartMissionPresenceDto dto)
    {
        var mission = await _db.PlanningMissions
            .Include(m => m.Residence)
            .FirstOrDefaultAsync(m => m.Id == dto.PlanningMissionId)
            ?? throw new InvalidOperationException("Mission introuvable.");

        var isValid = _geo.IsWithinRadius(
            dto.Latitude,
            dto.Longitude,
            mission.Residence.Latitude,
            mission.Residence.Longitude,
            mission.Residence.RayonAutoriseMetres);

        _db.Presences.Add(new Presence
        {
            UserId = userId,
            PlanningMissionId = mission.Id,
            HeureDebut = DateTime.UtcNow,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            IsGeoValidated = isValid,
            Anomalie = isValid ? null : "Hors zone autorisée"
        });

        await _db.SaveChangesAsync();
    }

    public async Task EndAsync(Guid userId, EndMissionPresenceDto dto)
    {
        var presence = await _db.Presences
            .Where(p => p.UserId == userId && p.HeureFin == null)
            .OrderByDescending(p => p.HeureDebut)
            .FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("Aucune présence active.");

        presence.HeureFin = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<PresenceMissionDto>> GetByMission(Guid missionId)
    {
        return await _db.Presences
            .Where(p => p.PlanningMissionId == missionId)
            .Select(p => new PresenceMissionDto
            {
                MissionId = p.PlanningMissionId!.Value,
                HeureDebut = p.HeureDebut,
                HeureFin = p.HeureFin,
                IsGeoValidated = p.IsGeoValidated,
                Anomalie = p.Anomalie
            })
            .ToListAsync();
    }

}
