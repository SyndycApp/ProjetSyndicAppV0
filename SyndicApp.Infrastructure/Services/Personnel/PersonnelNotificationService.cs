using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Infrastructure.SignalR;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PersonnelNotificationService : IPersonnelNotificationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHubContext<NotificationHub> _hub;

        public PersonnelNotificationService(
            ApplicationDbContext db,
            IHubContext<NotificationHub> hub)
        {
            _db = db;
            _hub = hub;
        }

        // ======================================================
        // 🔧 MÉTHODE CENTRALE (DB + TEMPS RÉEL)
        // ======================================================
        private async Task CreateAsync(Guid userId, string title, string message)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = title,
                Message = message,
                IsRead = false,
                SentAt = DateTime.UtcNow
            };

            _db.Notifications.Add(notification);
            await _db.SaveChangesAsync();

            // 🔔 PUSH TEMPS RÉEL (dashboard / futur mobile)
            await _hub.Clients
                .Group(userId.ToString())
                .SendAsync("notificationReceived", new
                {
                    notification.Id,
                    notification.Title,
                    notification.Message,
                    notification.SentAt,
                    notification.IsRead
                });
        }

        // ======================================================
        // 🔔 Mission imminente
        // ======================================================
        public async Task MissionImminenteAsync(
            Guid userId,
            DateOnly date,
            TimeSpan heureDebut)
        {
            await CreateAsync(
                userId,
                "Mission imminente",
                $"Votre mission du {date:dd/MM/yyyy} commence à {heureDebut:hh\\:mm}."
            );
        }

        // ======================================================
        // ⏱️ Retard détecté
        // ======================================================
        public async Task RetardDetecteAsync(Guid userId)
        {
            await CreateAsync(
                userId,
                "⏰ Retard détecté",
                "Votre prise de service a dépassé l’heure prévue."
            );
        }

        // ======================================================
        // ❌ Absence détectée
        // ======================================================
        public async Task AbsenceDetecteeAsync(Guid userId, DateOnly date)
        {
            await CreateAsync(
                userId,
                "❌ Absence détectée",
                $"Aucune présence enregistrée le {date:dd/MM/yyyy}."
            );
        }

        // ======================================================
        // ⚠️ Mission non validée (simple)
        // ======================================================
        public async Task MissionNonValideeAsync(Guid syndicUserId)
        {
            await CreateAsync(
                syndicUserId,
                "⚠️ Mission non validée",
                "Une mission n’a pas été validée géographiquement."
            );
        }

        // ======================================================
        // 🚨 Mission non validée (avec mission)
        // ======================================================
        public async Task MissionNonValideeAsync(
            Guid syndicUserId,
            Guid planningMissionId)
        {
            await CreateAsync(
                syndicUserId,
                "🚨 Mission non validée",
                $"La mission {planningMissionId} a été commencée hors zone autorisée."
            );
        }

        // ======================================================
        // 📥 Notifications utilisateur
        // ======================================================
        public async Task<IReadOnlyList<Notification>> GetMyNotificationsAsync(Guid userId)
        {
            return await _db.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

        // ======================================================
        // ✅ Marquer comme lue
        // ======================================================
        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var notif = await _db.Notifications.FindAsync(notificationId);
            if (notif == null) return;

            notif.IsRead = true;
            await _db.SaveChangesAsync();
        }
    }
}
