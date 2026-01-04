using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Application.Interfaces.Common;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Infrastructure;

public class RelanceVoteService : IRelanceVoteService
{
    private readonly ApplicationDbContext _db;
    private readonly INotificationService _notificationService;
    private readonly IAssembleeAccessPolicy _policy;

    public RelanceVoteService(
        ApplicationDbContext db,
        INotificationService notificationService,
        IAssembleeAccessPolicy policy)
    {
        _db = db;
        _notificationService = notificationService;
        _policy = policy;
    }

    public async Task RelancerNonVotantsAsync(Guid assembleeId, Guid syndicId)
    {
        var ag = await _db.AssembleesGenerales
            .FirstOrDefaultAsync(a => a.Id == assembleeId);

        if (ag == null)
            throw new InvalidOperationException("Assemblée introuvable.");

        if (!_policy.PeutVoter(ag) ||
            !_policy.EstDansPlageHoraireVote(ag, DateTime.UtcNow))
            throw new InvalidOperationException("Relance non autorisée.");

        var tousUsers = await _db.AffectationsLots
            .Where(a => a.Lot.ResidenceId == ag.ResidenceId)
            .Select(a => a.UserId)
            .Distinct()
            .ToListAsync();

        var ontVote = await _db.Votes
            .Where(v => v.Resolution.AssembleeGeneraleId == assembleeId)
            .Select(v => v.UserId)
            .Distinct()
            .ToListAsync();

        var dejaRelances = await _db.RelanceVoteLogs
            .Where(r =>
                r.AssembleeGeneraleId == assembleeId &&
                r.Type == "NON_VOTANT")
            .Select(r => r.UserId)
            .ToListAsync();

        var cibles = tousUsers
            .Except(ontVote)
            .Except(dejaRelances)
            .ToList();

        foreach (var userId in cibles)
        {
            await _notificationService.NotifierAsync(
                userId,
                "Vote en attente",
                $"Votre vote pour l’assemblée « {ag.Titre} » est toujours en attente.",
                "RELANCE_VOTE",
                ag.Id,
                "Vote"
            );

            _db.RelanceVoteLogs.Add(new RelanceVoteLog
            {
                AssembleeGeneraleId = assembleeId,
                UserId = userId
            });
        }

        // 🔍 AUDIT
        _db.AuditLogs.Add(new AuditLog
        {
            UserId = syndicId,
            Action = "RELANCE_NON_VOTANTS",
            Cible = $"Assemblee:{assembleeId}",
            DateAction = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }
}
