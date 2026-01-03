using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;


namespace SyndicApp.Infrastructure.Services.Assemblees;
public class ArchivageAssembleeService : IArchivageAssembleeService
{
    private readonly ApplicationDbContext _db;
    private readonly IAssembleeAccessPolicy _policy;

    public ArchivageAssembleeService(
        ApplicationDbContext db,
        IAssembleeAccessPolicy policy)
    {
        _db = db;
        _policy = policy;
    }

    public async Task ArchiverAsync(Guid assembleeId, Guid syndicId)
    {
        var ag = await _db.AssembleesGenerales
            .FirstOrDefaultAsync(a => a.Id == assembleeId);

        if (ag == null)
            throw new InvalidOperationException("Assemblée introuvable");

        if (!_policy.PeutArchiver(ag))
            throw new InvalidOperationException(
                "Cette assemblée ne peut pas être archivée."
            );

        ag.EstArchivee = true;
        ag.DateArchivage = DateTime.UtcNow;

        _db.AuditLogs.Add(new AuditLog
        {
            UserId = syndicId,
            Action = "ARCHIVAGE_AG",
            Cible = $"Assemblee:{assembleeId}",
            DateAction = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }

    public async Task SupprimerAsync(Guid assembleeId, Guid syndicId)
    {
        var ag = await _db.AssembleesGenerales
            .FirstOrDefaultAsync(a => a.Id == assembleeId);

        if (ag == null)
            throw new InvalidOperationException("Assemblée introuvable");

        if (!_policy.PeutSupprimer(ag))
            throw new InvalidOperationException(
                "Seules les assemblées en brouillon peuvent être supprimées."
            );

        _db.AssembleesGenerales.Remove(ag);

        _db.AuditLogs.Add(new AuditLog
        {
            UserId = syndicId,
            Action = "SUPPRESSION_AG",
            Cible = $"Assemblee:{assembleeId}",
            DateAction = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }
}
