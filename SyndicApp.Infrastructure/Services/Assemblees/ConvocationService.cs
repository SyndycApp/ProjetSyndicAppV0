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

        public ConvocationService(ApplicationDbContext db, IMailService mailService)
        {
            _db = db;
            _mailService = mailService;
        }

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
                        Email = u.Email
                    }
                )
                .ToListAsync();

            foreach (var dest in nonLecteurs)
            {
                await _mailService.EnvoyerAsync(
                    dest.Email,
                    "Relance Convocation",
                    "Merci de consulter la convocation."
                );

                // (optionnel mais recommandé)
                await _db.ConvocationDestinataires
                    .Where(d =>
                        d.ConvocationId == convocationId &&
                        d.UserId == dest.UserId)
                    .ExecuteUpdateAsync(s =>
                        s.SetProperty(x => x.RelanceLe, DateTime.UtcNow));
            }
        }


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


        public async Task EnvoyerEmailsAsync(Guid convocationId)
        {
            var convocation = await _db.Convocations
                .FirstOrDefaultAsync(c => c.Id == convocationId);

            if (convocation == null)
                throw new InvalidOperationException("Convocation introuvable");

            var destinataires = await _db.ConvocationDestinataires
                .Where(d => d.ConvocationId == convocationId)
                .Join(
                    _db.Users,
                    d => d.UserId,
                    u => u.Id,
                    (d, u) => new
                    {
                        d.UserId,
                        Email = u.Email
                    }
                )
                .ToListAsync();

            foreach (var dest in destinataires)
            {
                try
                {
                    await _mailService.EnvoyerAsync(
                        dest.Email,
                        "Convocation Assemblée Générale",
                        convocation.Contenu
                    );

                    _db.ConvocationEnvoiLogs.Add(new ConvocationEnvoiLog
                    {
                        ConvocationId = convocationId,
                        UserId = dest.UserId,
                        Email = dest.Email,
                        DateEnvoi = DateTime.UtcNow,
                        Succes = true
                    });
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

        public async Task SendAsync(CreateConvocationDto dto)
        {
            // 🧠 Choix du contenu
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
