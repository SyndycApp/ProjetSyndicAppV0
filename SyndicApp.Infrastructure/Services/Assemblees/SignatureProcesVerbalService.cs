using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Application.Interfaces.Common;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;
using SyndicApp.Infrastructure.Persistence;

namespace SyndicApp.Infrastructure.Services.Assemblees;

public class SignatureProcesVerbalService : ISignatureProcesVerbalService
{
    private readonly ApplicationDbContext _db;
    private readonly INotificationService _notificationService;

    public SignatureProcesVerbalService(
        ApplicationDbContext db,
        INotificationService notificationService)
    {
        _db = db;
        _notificationService = notificationService;
    }

    public async Task DemarrerWorkflowAsync(Guid procesVerbalId, List<Guid> signatairesIds)
    {
        var pv = await _db.ProcesVerbaux
            .Include(p => p.Signatures)
            .FirstOrDefaultAsync(p => p.Id == procesVerbalId);

        if (pv == null)
            throw new InvalidOperationException("PV introuvable.");

        if (pv.Statut != StatutProcesVerbal.Brouillon)
            throw new InvalidOperationException("Workflow déjà démarré.");

        int ordre = 1;

        foreach (var userId in signatairesIds.Distinct())
        {
            pv.Signatures.Add(new SignatureProcesVerbal
            {
                ProcesVerbalId = procesVerbalId,
                UserId = userId,
                OrdreSignature = ordre++
            });

            await _notificationService.NotifierAsync(
                userId,
                "Signature requise",
                "Un procès-verbal est en attente de votre signature.",
                "PV_SIGNATURE_REQUISE",
                procesVerbalId,
                "PROCES_VERBAL"
            );
        }

        pv.Statut = StatutProcesVerbal.EnValidation;

        await _db.SaveChangesAsync();
    }

    public async Task SignerAsync(Guid procesVerbalId, Guid userId, string? commentaire)
    {
        var pv = await _db.ProcesVerbaux
            .Include(p => p.Signatures)
            .FirstOrDefaultAsync(p => p.Id == procesVerbalId);

        if (pv == null)
            throw new InvalidOperationException("PV introuvable.");

        var signature = pv.Signatures
            .FirstOrDefault(s => s.UserId == userId);

        if (signature == null)
            throw new UnauthorizedAccessException("Signature non autorisée.");

        if (signature.EstSigne)
            throw new InvalidOperationException("Déjà signé.");

        var ordreMinNonSigne = pv.Signatures
            .Where(s => !s.EstSigne)
            .Min(s => s.OrdreSignature);

        if (signature.OrdreSignature != ordreMinNonSigne)
            throw new InvalidOperationException("Signature hors ordre.");

        signature.EstSigne = true;
        signature.DateSignature = DateTime.UtcNow;
        signature.Commentaire = commentaire;

        pv.Statut = pv.Signatures.All(s => s.EstSigne)
            ? StatutProcesVerbal.SigneDefinitivement
            : StatutProcesVerbal.SignePartiellement;

        if (pv.Statut == StatutProcesVerbal.SigneDefinitivement)
        {
            foreach (var s in pv.Signatures)
            {
                await _notificationService.NotifierAsync(
                    s.UserId,
                    "PV signé définitivement",
                    "Le procès-verbal est désormais officiel.",
                    "PV_SIGNE_COMPLET",
                    procesVerbalId,
                    "PROCES_VERBAL"
                );
            }
        }

        await _db.SaveChangesAsync();
    }

    public async Task<ProcesVerbalEtatDto> GetEtatAsync(Guid procesVerbalId)
    {
        var pv = await _db.ProcesVerbaux
            .Include(p => p.Signatures)
            .FirstOrDefaultAsync(p => p.Id == procesVerbalId);

        if (pv == null)
            throw new InvalidOperationException("PV introuvable.");

        var signatures = pv.Signatures
            .OrderBy(s => s.OrdreSignature)
            .Select(s => new SignatureProcesVerbalDto(
                s.UserId,
                s.UserId.ToString(), // à mapper vers nom réel plus tard
                s.OrdreSignature,
                s.EstSigne,
                s.DateSignature
            ))
            .ToList();

        return new ProcesVerbalEtatDto(
            pv.Id,
            pv.Statut,
            pv.Statut == StatutProcesVerbal.SigneDefinitivement,
            signatures
        );
    }
}
