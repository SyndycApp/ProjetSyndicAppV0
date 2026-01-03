using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;
using SyndicApp.Infrastructure;

public class AnnotationAssembleeService : IAnnotationAssembleeService
{
    private readonly ApplicationDbContext _db;

    public AnnotationAssembleeService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AjouterAsync(Guid assembleeId, Guid syndicId, CreateAnnotationDto dto)
    {
        var ag = await _db.AssembleesGenerales
            .FirstOrDefaultAsync(a => a.Id == assembleeId);

        if (ag == null)
            throw new InvalidOperationException("Assemblée introuvable.");

        if (ag.Statut != StatutAssemblee.Cloturee)
            throw new InvalidOperationException(
                "Les annotations sont autorisées uniquement après clôture."
            );

        _db.AnnotationsAssemblee.Add(new AnnotationAssemblee
        {
            AssembleeGeneraleId = assembleeId,
            AuteurId = syndicId,
            Contenu = dto.Contenu
        });

        _db.AuditLogs.Add(new AuditLog
        {
            UserId = syndicId,
            Action = "AJOUT_ANNOTATION_AG",
            Cible = $"Assemblee:{assembleeId}",
            DateAction = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }

    public async Task ModifierAsync(Guid annotationId, Guid syndicId, CreateAnnotationDto dto)
    {
        var annotation = await _db.AnnotationsAssemblee
            .FirstOrDefaultAsync(a => a.Id == annotationId);

        if (annotation == null)
            throw new InvalidOperationException("Annotation introuvable.");

        if (annotation.AuteurId != syndicId)
            throw new InvalidOperationException("Action non autorisée.");

        annotation.Contenu = dto.Contenu;
        annotation.DateModification = DateTime.UtcNow;

        _db.AuditLogs.Add(new AuditLog
        {
            UserId = syndicId,
            Action = "MODIFICATION_ANNOTATION_AG",
            Cible = $"Annotation:{annotationId}",
            DateAction = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }

    public async Task SupprimerAsync(Guid annotationId, Guid syndicId)
    {
        var annotation = await _db.AnnotationsAssemblee
            .FirstOrDefaultAsync(a => a.Id == annotationId);

        if (annotation == null)
            throw new InvalidOperationException("Annotation introuvable.");

        if (annotation.AuteurId != syndicId)
            throw new InvalidOperationException("Action non autorisée.");

        _db.AnnotationsAssemblee.Remove(annotation);

        _db.AuditLogs.Add(new AuditLog
        {
            UserId = syndicId,
            Action = "SUPPRESSION_ANNOTATION_AG",
            Cible = $"Annotation:{annotationId}",
            DateAction = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }

    public async Task<List<AnnotationDto>> GetByAssembleeAsync(Guid assembleeId)
    {
        return await _db.AnnotationsAssemblee
            .Where(a => a.AssembleeGeneraleId == assembleeId)
            .OrderByDescending(a => a.DateCreation)
            .Select(a => new AnnotationDto(
                a.Id,
                a.Contenu,
                a.DateCreation,
                a.DateModification
            ))
            .ToListAsync();
    }
}
