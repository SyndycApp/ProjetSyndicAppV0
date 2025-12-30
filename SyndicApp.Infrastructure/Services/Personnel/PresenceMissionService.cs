using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Domain.Entities.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel;

public class PresenceMissionService : IPresenceMissionService
{
    private readonly ApplicationDbContext _db;
    private readonly IGeoPresenceService _geo;
    private readonly IPersonnelNotificationService _notification;

    public PresenceMissionService(ApplicationDbContext db, IGeoPresenceService geo, IPersonnelNotificationService notification)
    {
        _db = db;
        _geo = geo;
        _notification = notification;
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

        var now = DateTime.UtcNow;

        // 🕒 Heure théorique de début
        var heureTheorique = mission.Date.ToDateTime(
            TimeOnly.FromTimeSpan(mission.HeureDebut));

        // ⏱️ RETARD (>10 min)
        if (now > heureTheorique.AddMinutes(15))
        {
            await _notification.RetardDetecteAsync(userId);
        }

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

    public async Task<IReadOnlyList<TempsTravailJourDto>> GetTempsTravailParJourAsync(Guid employeUserId)
    {
        var presences = await _db.Presences
            .AsNoTracking()
            .Where(p =>
                p.UserId == employeUserId &&
                p.IsGeoValidated == true &&
                p.HeureDebut != null &&
                p.HeureFin != null)
            .ToListAsync();

        var result = presences
            .GroupBy(p => DateOnly.FromDateTime(p.HeureDebut!.Value))
            .Select(g => new TempsTravailJourDto
            {
                Date = g.Key,
                HeuresTravaillees = g.Sum(p =>
                    (p.HeureFin!.Value - p.HeureDebut!.Value).TotalHours)
            })
            .OrderBy(r => r.Date)
            .ToList();

        return result;
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
