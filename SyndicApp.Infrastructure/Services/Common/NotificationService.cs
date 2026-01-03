using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Common;
using SyndicApp.Application.Interfaces.Common;
using SyndicApp.Domain.Entities.Common;
using SyndicApp.Infrastructure;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _db;

    public NotificationService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task NotifierAsync(
        Guid userId,
        string titre,
        string message,
        string type,
        Guid? cibleId = null,
        string? cibleType = null)
    {
        _db.Notifications.Add(new Notification
        {
            UserId = userId,
            Title = titre,
            Message = message,
            Type = type,
            CibleId = cibleId,
            CibleType = cibleType
        });

        await _db.SaveChangesAsync();
    }

    public async Task<List<NotificationDto>> GetMesNotificationsAsync(Guid userId)
    {
        return await _db.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.SentAt)
            .Select(n => new NotificationDto(
                n.Id,
                n.Title,
                n.Message,
                n.Type,
                n.IsRead,
                n.SentAt
            ))
            .ToListAsync();
    }

    public async Task MarquerCommeLueAsync(Guid notificationId, Guid userId)
    {
        var notif = await _db.Notifications
            .FirstOrDefaultAsync(n =>
                n.Id == notificationId &&
                n.UserId == userId);

        if (notif == null)
            throw new InvalidOperationException("Notification introuvable");

        notif.IsRead = true;
        await _db.SaveChangesAsync();
    }
}
