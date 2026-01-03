using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Application.Interfaces.Common;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;
using SyndicApp.Infrastructure;

public class RappelAssembleeService : IRappelAssembleeService
{
    private static readonly int[] JOURS_RAPPEL = { 15, 7, 1 };

    private readonly ApplicationDbContext _db;
    private readonly INotificationService _notificationService;

    public RappelAssembleeService(
        ApplicationDbContext db,
        INotificationService notificationService)
    {
        _db = db;
        _notificationService = notificationService;
    }

    public async Task ExecuterAsync()
    {
        var now = DateTime.UtcNow.Date;

        var assemblees = await _db.AssembleesGenerales
            .Where(a =>
                a.Statut == StatutAssemblee.Publiee ||
                a.Statut == StatutAssemblee.Ouverte)
            .ToListAsync();

        foreach (var ag in assemblees)
        {
            foreach (var jours in JOURS_RAPPEL)
            {
                var dateCible = ag.DateDebut.Date.AddDays(-jours);

                if (dateCible != now)
                    continue;

                var dejaEnvoye = await _db.AssembleeRappels.AnyAsync(r =>
                    r.AssembleeGeneraleId == ag.Id &&
                    r.JoursAvant == jours);

                if (dejaEnvoye)
                    continue;

                // 🔔 destinataires = copropriétaires
                var userIds = await _db.AffectationsLots
                    .Where(a => a.Lot.ResidenceId == ag.ResidenceId)
                    .Select(a => a.UserId)
                    .Distinct()
                    .ToListAsync();

                foreach (var userId in userIds)
                {
                    await _notificationService.NotifierAsync(
                        userId,
                        "Rappel Assemblée Générale",
                        $"L’assemblée « {ag.Titre} » aura lieu dans {jours} jour(s).",
                        "RAPPEL_AG",
                        ag.Id,
                        "Assemblee"
                    );
                }

                _db.AssembleeRappels.Add(new AssembleeRappel
                {
                    AssembleeGeneraleId = ag.Id,
                    JoursAvant = jours,
                    DateEnvoi = DateTime.UtcNow
                });
            }
        }

        await _db.SaveChangesAsync();
    }
}
