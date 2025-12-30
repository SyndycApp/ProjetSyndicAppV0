using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel;

public class PersonnelReminderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PersonnelReminderService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var notifier = scope.ServiceProvider.GetRequiredService<IPersonnelNotificationService>();

            var now = DateTime.UtcNow;
            var today = DateOnly.FromDateTime(now);

            // ============================
            // 🔔 MISSIONS IMMINENTES (30 min)
            // ============================
            var missionsImminentes = await db.PlanningMissions
                .Where(m =>
                    m.Date == today &&
                    m.HeureDebut > TimeSpan.FromHours(now.Hour)
                    && m.HeureDebut <= TimeSpan.FromHours(now.AddMinutes(30).Hour))
                .ToListAsync(stoppingToken);

            foreach (var mission in missionsImminentes)
            {
                var userId = await db.Employes
                    .Where(e => e.Id == mission.EmployeId)
                    .Select(e => e.UserId)
                    .FirstOrDefaultAsync(stoppingToken);

                if (userId != Guid.Empty)
                {
                    await notifier.MissionImminenteAsync(
                        userId,
                        mission.Date,
                        mission.HeureDebut);
                }
            }

            // ============================
            // ❌ ABSENCES JOURNALIÈRES
            // ============================
            var missionsDuJour = await db.PlanningMissions
                .Where(m => m.Date == today)
                .ToListAsync(stoppingToken);

            foreach (var mission in missionsDuJour)
            {
                var userId = await db.Employes
                    .Where(e => e.Id == mission.EmployeId)
                    .Select(e => e.UserId)
                    .FirstOrDefaultAsync(stoppingToken);

                var hasPresence = await db.Presences
                    .AnyAsync(p => p.PlanningMissionId == mission.Id, stoppingToken);

                var hasJustification = await db.AbsenceJustifications
                    .AnyAsync(a =>
                        a.UserId == userId &&
                        a.Date == today &&
                        a.Validee, stoppingToken);

                if (!hasPresence && !hasJustification)
                {
                    await notifier.AbsenceDetecteeAsync(userId, today);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
