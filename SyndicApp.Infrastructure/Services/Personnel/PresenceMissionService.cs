using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Domain.Entities.Personnel;
using Microsoft.Extensions.Options;
using SyndicApp.Application.Config;


namespace SyndicApp.Infrastructure.Services.Personnel;

public class PresenceMissionService : IPresenceMissionService
{
    private readonly ApplicationDbContext _db;
    private readonly IGeoPresenceService _geo;
    private readonly IPersonnelNotificationService _notification;
    private readonly PresenceGeoOptions _geoOptions;

    public PresenceMissionService(ApplicationDbContext db, IGeoPresenceService geo, IPersonnelNotificationService notification, IOptions<PresenceGeoOptions> geoOptions)
    {
        _db = db;
        _geo = geo;
        _notification = notification;
        _geoOptions = geoOptions.Value;
    }

    public async Task StartAsync(Guid userId, StartMissionPresenceDto dto)
    {
        var mission = await _db.PlanningMissions
            .Include(m => m.Residence)
            .FirstOrDefaultAsync(m => m.Id == dto.PlanningMissionId)
            ?? throw new InvalidOperationException("Mission introuvable.");

        if (mission.Employe.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                "Cette mission ne vous appartient pas.");
        }

        var isValid = _geo.IsWithinRadius(
            dto.Latitude,
            dto.Longitude,
            mission.Residence.Latitude,
            mission.Residence.Longitude,
            mission.Residence.RayonAutoriseMetres + _geoOptions.ToleranceMetres);

        if (!isValid && _geoOptions.Mode == "Strict")
        {
            await _notification.MissionNonValideeAsync(
                mission.Employe.UserId,
                mission.Id);

            throw new InvalidOperationException(
                "Pointage refusé : vous êtes hors zone autorisée.");
        }

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
            Anomalie = isValid ? "Pointage validé" : _geoOptions.Mode == "Informative" ? "Hors zone (non bloquant)" : "Hors zone autorisée"
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
