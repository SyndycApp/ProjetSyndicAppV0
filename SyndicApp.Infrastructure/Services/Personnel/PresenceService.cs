using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel;

public class PresenceService : IPresenceService
{
    private readonly ApplicationDbContext _db;

    public PresenceService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task StartAsync(Guid userId, StartPresenceDto dto)
    {
        var today = DateTime.UtcNow.Date;

        var alreadyStarted = await _db.Presences.AnyAsync(p =>
            p.UserId == userId &&
            p.Date == today &&
            p.HeureFin == null);

        if (alreadyStarted)
            throw new InvalidOperationException("Présence déjà démarrée.");

        _db.Presences.Add(new Presence
        {
            UserId = userId,
            Date = today,
            HeureDebut = DateTime.UtcNow,
            ResidenceNom = dto.ResidenceNom
        });

        await _db.SaveChangesAsync();
    }

    public async Task EndAsync(Guid userId)
    {
        var presence = await _db.Presences
            .Where(p => p.UserId == userId && p.HeureFin == null)
            .OrderByDescending(p => p.HeureDebut)
            .FirstOrDefaultAsync();

        if (presence == null)
            throw new InvalidOperationException("Aucune présence en cours.");

        presence.HeureFin = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<PresenceDto>> GetMyHistoryAsync(Guid userId)
    {
        return await _db.Presences
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.Date)
            .Select(p => new PresenceDto
            {
                Id = p.Id,
                Date = p.Date,
                HeureDebut = p.HeureDebut,
                HeureFin = p.HeureFin,
                ResidenceNom = p.ResidenceNom
            })
            .ToListAsync();
    }
}
