using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Application.Interfaces.Common;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;
using SyndicApp.Infrastructure.Services.Common;
using System.Text;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class ProcesVerbalService : IProcesVerbalService
    {
        private readonly ApplicationDbContext _db;
        private readonly INotificationService _notificationService;

        public ProcesVerbalService(ApplicationDbContext db, INotificationService notificationService)
        {
            _db = db;
            _notificationService = notificationService;
        }

        public async Task ScellerVersionAsync(Guid procesVerbalVersionId, Guid syndicId)
        {
            var version = await _db.ProcesVerbalVersions
                .Include(v => v.ProcesVerbal)
                .FirstOrDefaultAsync(v => v.Id == procesVerbalVersionId);

            if (version == null)
                throw new InvalidOperationException("Version introuvable.");

            if (version.EstScelle)
                throw new InvalidOperationException("Cette version est déjà scellée.");

            var payload =
                $"{version.ProcesVerbalId}|{version.NumeroVersion}|{version.Contenu}|{version.DateGeneration:o}";

            version.HashScellage = HashService.ComputeSha256(payload);

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                version.UrlPdf
            );

            if (!File.Exists(fullPath))
                throw new InvalidOperationException("Fichier PDF introuvable pour scellage.");

            version.HashPdf = CalculerHashPdf(fullPath);


            version.DateScellage = DateTime.UtcNow;
            version.ScelleParId = syndicId;
            version.EstScelle = true;

            version.ProcesVerbal.EstVerrouille = true;

            _db.AuditLogs.Add(new AuditLog
            {
                UserId = syndicId,
                Action = "SCELLAGE_PV",
                Cible = $"ProcesVerbalVersion:{procesVerbalVersionId}",
                DateAction = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }

        public async Task AjouterCommentaireAsync(Guid procesVerbalVersionId, string commentaire, Guid syndicId)
        {
            var version = await _db.ProcesVerbalVersions
                .Include(v => v.ProcesVerbal)
                .ThenInclude(pv => pv.AssembleeGenerale)
                .FirstOrDefaultAsync(v => v.Id == procesVerbalVersionId);

            if (version == null)
                throw new InvalidOperationException("Version de procès-verbal introuvable.");

            if (version.EstScelle)
                throw new InvalidOperationException(
                    "PV scellé juridiquement. Annotation interdite."
                );


            if (version.ProcesVerbal.EstVerrouille)
                throw new InvalidOperationException(
                    "Le procès-verbal est verrouillé. Annotation impossible."
                );

            if (string.IsNullOrWhiteSpace(commentaire))
                throw new InvalidOperationException("Le commentaire est vide.");


            version.Contenu +=
                $"\n\n--- COMMENTAIRE SYNDIC ({DateTime.UtcNow:dd/MM/yyyy HH:mm}) ---\n{commentaire}";

            _db.AuditLogs.Add(new AuditLog
            {
                UserId = syndicId,
                Action = "ANNOTATION_PV",
                Cible = $"ProcesVerbalVersion:{procesVerbalVersionId}",
                DateAction = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }

        private static string CalculerHashPdf(string fullPath)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            using var stream = File.OpenRead(fullPath);
            var hash = sha.ComputeHash(stream);
            return Convert.ToHexString(hash);
        }

        public async Task GenerateAsync(Guid assembleeId, Guid syndicId)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            // =========================
            // 🔎 CHARGER AG + RÉSOLUTIONS + DÉCISIONS
            // =========================
            var ag = await _db.AssembleesGenerales
                .Include(a => a.Resolutions)
                    .ThenInclude(r => r.Decision)
                .FirstOrDefaultAsync(a => a.Id == assembleeId);

            if (ag == null)
                throw new InvalidOperationException("Assemblée introuvable.");

            if (ag.Statut != StatutAssemblee.Cloturee)
                throw new InvalidOperationException(
                    "Le PV ne peut être généré que pour une AG clôturée."
                );

            var contenu = new StringBuilder();

            contenu.AppendLine("PROCÈS-VERBAL D’ASSEMBLÉE GÉNÉRALE");
            contenu.AppendLine($"Titre : {ag.Titre}");
            contenu.AppendLine($"Date : {ag.DateFin:dd/MM/yyyy}");
            contenu.AppendLine($"Année : {ag.Annee}");
            contenu.AppendLine(new string('-', 40));

            foreach (var res in ag.Resolutions)
            {
                if (res.Decision == null)
                    continue;

                var d = res.Decision;

                contenu.AppendLine($"Résolution : {res.Titre}");
                contenu.AppendLine($"Pour : {d.TotalPour}");
                contenu.AppendLine($"Contre : {d.TotalContre}");
                contenu.AppendLine($"Abstention : {d.TotalAbstention}");
                contenu.AppendLine(
                    $"Décision : {(d.EstAdoptee ? "ADOPTÉE" : "REJETÉE")}"
                );
                contenu.AppendLine(new string('-', 30));
            }

            var root = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "documents",
                "pv"
            );

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            var pv = await _db.ProcesVerbaux
                .FirstOrDefaultAsync(p => p.AssembleeGeneraleId == assembleeId);

            if (pv != null && pv.EstVerrouille)
                throw new InvalidOperationException(
                    "PV scellé juridiquement. Génération interdite."
                );


            if (pv == null)
            {
                pv = new ProcesVerbal
                {
                    AssembleeGeneraleId = assembleeId,
                    NumeroPV = $"PV-{ag.Annee}-{assembleeId.ToString()[..6]}",
                    GenereParId = syndicId,
                    DateGeneration = DateTime.UtcNow,
                    EstVerrouille = false,
                    EstArchive = false
                };

                _db.ProcesVerbaux.Add(pv);
                await _db.SaveChangesAsync();
            }
            var derniereVersion = await _db.ProcesVerbalVersions
                .Where(v => v.ProcesVerbalId == pv.Id)
                .OrderByDescending(v => v.NumeroVersion)
                .Select(v => v.NumeroVersion)
                .FirstOrDefaultAsync();

            var numeroVersion = derniereVersion + 1;

            await _db.ProcesVerbalVersions
                .Where(v =>
                    v.ProcesVerbalId == pv.Id &&
                    v.EstOfficielle)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(v => v.EstOfficielle, false));

            var fileName = $"PV_{ag.Annee}_{assembleeId}_v{numeroVersion}.pdf";
            var fullPath = Path.Combine(root, fileName);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Header()
                        .Text("PROCÈS-VERBAL D’ASSEMBLÉE GÉNÉRALE")
                        .FontSize(18)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"Assemblée : {ag.Titre}");
                        col.Item().Text($"Date : {ag.DateFin:dd/MM/yyyy}");
                        col.Item().LineHorizontal(1);

                        foreach (var res in ag.Resolutions)
                        {
                            if (res.Decision == null)
                                continue;

                            var d = res.Decision;

                            col.Item().Text(res.Titre).Bold();
                            col.Item().Text($"Pour : {d.TotalPour}");
                            col.Item().Text($"Contre : {d.TotalContre}");
                            col.Item().Text($"Abstention : {d.TotalAbstention}");
                            col.Item()
                                .Text(d.EstAdoptee ? "ADOPTÉE" : "REJETÉE")
                                .Bold()
                                .FontColor(
                                    d.EstAdoptee
                                        ? Colors.Green.Medium
                                        : Colors.Red.Medium
                                );
                            col.Item().LineHorizontal(0.5f);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("SyndicApp • Document officiel");
                });
            }).GeneratePdf(fullPath);


            _db.ProcesVerbalVersions.Add(new ProcesVerbalVersion
            {
                ProcesVerbalId = pv.Id,
                NumeroVersion = numeroVersion,
                Contenu = contenu.ToString(),
                UrlPdf = $"uploads/documents/pv/{fileName}",
                EstOfficielle = true,
                DateGeneration = DateTime.UtcNow,
                GenereParId = syndicId
            });
            
            _db.AuditLogs.Add(new AuditLog
            {
                UserId = syndicId,
                Action = "GENERATION_PV_VERSION",
                Cible = $"Assemblee:{assembleeId}/Version:{numeroVersion}",
                DateAction = DateTime.UtcNow
            });
            

            await _db.SaveChangesAsync();


            var userIds = await _db.AffectationsLots
                .Where(a => a.Lot.ResidenceId == ag.ResidenceId)
                .Select(a => a.UserId)
                .Distinct()
                .ToListAsync();

            foreach (var userId in userIds)
            {
                await _notificationService.NotifierAsync(
                    userId,
                    "Procès-verbal disponible",
                    $"Le procès-verbal de l’assemblée « {ag.Titre} » est disponible.",
                    "PROCES_VERBAL",
                    ag.Id,
                    "ProcesVerbal"
                );
            }
        }

        public async Task<List<ProcesVerbalVersionDto>> GetVersionsAsync(Guid assembleeId)
        {
            return await _db.ProcesVerbalVersions
                .Where(v => v.ProcesVerbal.AssembleeGeneraleId == assembleeId)
                .OrderByDescending(v => v.NumeroVersion)
                .Select(v => new ProcesVerbalVersionDto(
                    v.NumeroVersion,
                    v.EstOfficielle,
                    v.DateGeneration,
                    v.UrlPdf
                ))
                .ToListAsync();
        }

        public async Task<(byte[] Content, string FileName)> GetPdfAsync(Guid assembleeId)
        {
            // 🔎 Charger AG
            var ag = await _db.AssembleesGenerales
                .FirstOrDefaultAsync(a => a.Id == assembleeId);

            if (ag == null)
                throw new InvalidOperationException("Assemblée introuvable.");

            // 🔎 Charger la version officielle du PV
            var version = await _db.ProcesVerbalVersions
                .Where(v =>
                    v.ProcesVerbal.AssembleeGeneraleId == assembleeId &&
                    v.EstOfficielle)
                .OrderByDescending(v => v.NumeroVersion)
                .FirstOrDefaultAsync();

            if (version == null)
                throw new InvalidOperationException("Procès-verbal non disponible.");

            // 📁 Chemin réel du fichier (technique)
            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                version.UrlPdf.Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("PDF introuvable.", fullPath);

            var bytes = await File.ReadAllBytesAsync(fullPath);

            // 🧼 Nettoyage du nom AG (anti caractères interdits)
            string Safe(string value)
            {
                foreach (var c in Path.GetInvalidFileNameChars())
                    value = value.Replace(c, '_');

                return value.Replace(" ", "_");
            }

            // 📄 NOM MÉTIER DU FICHIER
            var fileName =
                $"PV_{Safe(ag.Titre)}_v{version.NumeroVersion}.pdf";

            return (bytes, fileName);
        }

        public async Task<VerifierIntegritePvDto> VerifierIntegriteAsync(Guid procesVerbalVersionId)
        {
            var version = await _db.ProcesVerbalVersions
                .Include(v => v.ProcesVerbal)
                .FirstOrDefaultAsync(v => v.Id == procesVerbalVersionId);

            if (version == null)
                throw new InvalidOperationException("Version de PV introuvable.");

            if (!version.EstScelle)
                throw new InvalidOperationException("Cette version n’est pas scellée.");

            var payload =
                $"{version.ProcesVerbalId}|{version.NumeroVersion}|{version.Contenu}|{version.DateGeneration:o}";

            var hashMetierCalcule = HashService.ComputeSha256(payload);
            var contenuValide = hashMetierCalcule == version.HashScellage;

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                version.UrlPdf
            );

            if (!File.Exists(fullPath))
                throw new InvalidOperationException("Fichier PDF introuvable.");

            var hashPdfCalcule = CalculerHashPdf(fullPath);

            var pdfValide = hashPdfCalcule == version.HashPdf;

            return new VerifierIntegritePvDto(
                    version.Id,
                    pdfValide && contenuValide,   
                    pdfValide,                   
                    contenuValide,                
                    version.HashScellage,         
                    hashMetierCalcule,             
                    DateTime.UtcNow
             );
        }

    }
}
