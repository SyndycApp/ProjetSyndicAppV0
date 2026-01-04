using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Application.Interfaces.Common;
using SyndicApp.Domain.Entities.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class ConvocationService : IConvocationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _env;
        private readonly INotificationService _notificationService;

        public ConvocationService(
            ApplicationDbContext db,
            IMailService mailService,
            IWebHostEnvironment env,
            INotificationService notificationService)
        {
            _db = db;
            _mailService = mailService;
            _env = env;
            _notificationService = notificationService;
        }

        // =====================================================
        // 🔁 RELANCE DES NON-LECTEURS
        // =====================================================
        public async Task RelancerNonLecteursAsync(Guid convocationId)
        {
            var nonLecteurs = await _db.ConvocationDestinataires
                .Where(d =>
                    d.ConvocationId == convocationId &&
                    d.LuLe == null)
                .Join(
                    _db.Users,
                    d => d.UserId,
                    u => u.Id,
                    (d, u) => new
                    {
                        d.UserId,
                        u.Email
                    }
                )
                .ToListAsync();

            foreach (var dest in nonLecteurs)
            {
                if (string.IsNullOrWhiteSpace(dest.Email))
                    continue;

                await _mailService.EnvoyerAsync(
                    dest.Email,
                    "Relance Convocation",
                    "<p>Merci de consulter la convocation.</p>",
                    isHtml: true
                );

                await _db.ConvocationDestinataires
                    .Where(d =>
                        d.ConvocationId == convocationId &&
                        d.UserId == dest.UserId)
                    .ExecuteUpdateAsync(s =>
                        s.SetProperty(x => x.RelanceLe, DateTime.UtcNow));
            }
        }

        // =====================================================
        // 👀 LECTEURS DE CONVOCATION
        // =====================================================
        public async Task<List<ConvocationLectureDto>> GetLecteursAsync(Guid convocationId)
        {
            return await _db.ConvocationDestinataires
                .Where(d => d.ConvocationId == convocationId)
                .Join(
                    _db.Users,
                    d => d.UserId,
                    u => u.Id,
                    (d, u) => new ConvocationLectureDto(
                        d.UserId,
                        u.FullName,
                        d.LuLe != null,
                        d.LuLe
                    )
                )
                .ToListAsync();
        }

        // =====================================================
        // 📧 ENVOI DES CONVOCATIONS
        // =====================================================
        public async Task EnvoyerEmailsAsync(Guid convocationId)
        {
            var convocation = await _db.Convocations
                .FirstOrDefaultAsync(c => c.Id == convocationId);

            if (convocation == null)
                throw new InvalidOperationException("Convocation introuvable");

            var ag = await _db.AssembleesGenerales
                .Include(a => a.OrdreDuJour)
                .FirstOrDefaultAsync(a => a.Id == convocation.AssembleeGeneraleId);

            if (ag == null)
                throw new InvalidOperationException("Assemblée introuvable");

            var residence = await _db.Residences
                .FirstOrDefaultAsync(r => r.Id == ag.ResidenceId);

            if (residence == null)
                throw new InvalidOperationException("Résidence introuvable");

            var contenuHtml = ConvocationContentBuilder.BuildHtml(
                _env,
                ag,
                residence,
                ag.OrdreDuJour,
                "SyndicApp"
            );

            var destinataires = await _db.ConvocationDestinataires
                .Where(d => d.ConvocationId == convocationId)
                .Join(
                    _db.Users,
                    d => d.UserId,
                    u => u.Id,
                    (d, u) => new
                    {
                        d.UserId,
                        u.Email
                    }
                )
                .ToListAsync();

            foreach (var dest in destinataires)
            {
                if (string.IsNullOrWhiteSpace(dest.Email))
                    continue;

                try
                {
                    await _mailService.EnvoyerAsync(
                        dest.Email,
                        "Convocation Assemblée Générale",
                        contenuHtml,
                        isHtml: true
                    );

                    _db.ConvocationEnvoiLogs.Add(new ConvocationEnvoiLog
                    {
                        ConvocationId = convocationId,
                        UserId = dest.UserId,
                        Email = dest.Email,
                        DateEnvoi = DateTime.UtcNow,
                        Succes = true
                    });

                    await _notificationService.NotifierAsync(
                        userId: dest.UserId,
                        titre: "Convocation Assemblée Générale",
                        message: $"Une convocation pour l’assemblée « {ag.Titre} » est disponible.",
                        type: "CONVOCATION",
                        cibleId: ag.Id,
                        cibleType: "Assemblee"
                    );
                }
                catch (Exception ex)
                {
                    _db.ConvocationEnvoiLogs.Add(new ConvocationEnvoiLog
                    {
                        ConvocationId = convocationId,
                        UserId = dest.UserId,
                        Email = dest.Email,
                        DateEnvoi = DateTime.UtcNow,
                        Succes = false,
                        Erreur = ex.Message
                    });
                }
            }

            await _db.SaveChangesAsync();
        }

        // =====================================================
        // 👁 MARQUER COMME LUE
        // =====================================================
        public async Task MarquerCommeLueAsync(Guid convocationId, Guid userId)
        {
            var dest = await _db.ConvocationDestinataires
                .FirstOrDefaultAsync(d =>
                    d.ConvocationId == convocationId &&
                    d.UserId == userId);

            if (dest == null)
                throw new InvalidOperationException("Convocation introuvable");

            if (dest.LuLe == null)
                dest.LuLe = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        // =====================================================
        // 📎 PIÈCE JOINTE
        // =====================================================
        public async Task AjouterPieceJointeAsync(Guid convocationId, string nomFichier, string urlFichier)
        {
            var convocationExiste = await _db.Convocations
                .AnyAsync(c => c.Id == convocationId);

            if (!convocationExiste)
                throw new InvalidOperationException("Convocation introuvable");

            _db.ConvocationPiecesJointes.Add(new ConvocationPieceJointe
            {
                ConvocationId = convocationId,
                NomFichier = nomFichier,
                UrlFichier = urlFichier
            });

            await _db.SaveChangesAsync();
        }

        // =====================================================
        // 🆕 CRÉATION + ENVOI
        // =====================================================
        public async Task SendAsync(CreateConvocationDto dto)
        {
            var contenu = dto.ModeleId != null
                ? await _db.ModelesConvocation
                    .Where(m => m.Id == dto.ModeleId)
                    .Select(m => m.Contenu)
                    .FirstAsync()
                : dto.Contenu;

            var convocation = new Convocation
            {
                AssembleeGeneraleId = dto.AssembleeGeneraleId,
                DateEnvoi = DateTime.UtcNow,
                Contenu = contenu
            };

            foreach (var userId in dto.DestinataireUserIds)
            {
                convocation.Destinataires.Add(new ConvocationDestinataire
                {
                    UserId = userId
                });
            }

            _db.Convocations.Add(convocation);
            await _db.SaveChangesAsync();
        }
    }
}
