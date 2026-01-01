using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;
using System.Text;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class ProcesVerbalService : IProcesVerbalService
    {
        private readonly ApplicationDbContext _db;

        public ProcesVerbalService(ApplicationDbContext db)
        {
            _db = db;
        }

        // =========================
        // 📄 GÉNÉRATION DU PV
        // =========================
        public async Task GenerateAsync(Guid assembleeId, Guid syndicId)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            // 🔎 Charger AG + Résolutions + Décisions
            var ag = await _db.AssembleesGenerales
                 .Include(a => a.Resolutions)
                    .ThenInclude(r => r.Decision)
                 .FirstOrDefaultAsync(a => a.Id == assembleeId);


            if (ag == null)
                throw new InvalidOperationException("Assemblée introuvable.");

            if (ag.Statut != StatutAssemblee.Cloturee)
                throw new InvalidOperationException("Le PV ne peut être généré que pour une AG clôturée.");

            // =========================
            // 🧠 CONTENU MÉTIER DU PV
            // =========================
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
                contenu.AppendLine($"Décision : {(d.EstAdoptee ? "ADOPTÉE" : "REJETÉE")}");
                contenu.AppendLine(new string('-', 30));
            }

            // =========================
            // 📁 CHEMIN PDF
            // =========================
            var root = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "documents",
                "pv"
            );

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            var fileName = $"PV_{ag.Annee}_{assembleeId}.pdf";
            var fullPath = Path.Combine(root, fileName);

            // =========================
            // 📄 GÉNÉRATION PDF
            // =========================
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
                            col.Item().Text(d.EstAdoptee ? "ADOPTÉE" : "REJETÉE")
                                .Bold()
                                .FontColor(d.EstAdoptee ? Colors.Green.Medium : Colors.Red.Medium);
                            col.Item().LineHorizontal(0.5f);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("SyndicApp • Document officiel");
                });
            }).GeneratePdf(fullPath);

            // =========================
            // 💾 SAUVEGARDE BDD
            // =========================
            var pv = await _db.ProcesVerbaux
                .FirstOrDefaultAsync(p => p.AssembleeGeneraleId == assembleeId);

            if (pv == null)
            {
                pv = new ProcesVerbal
                {
                    AssembleeGeneraleId = assembleeId,
                    DateGeneration = DateTime.UtcNow,
                    Contenu = contenu.ToString(),
                    NumeroPV = $"PV-{ag.Annee}-{assembleeId.ToString()[..6]}",
                    GenereParId = syndicId,
                    UrlPdf = $"uploads/documents/pv/{fileName}",
                    EstVerrouille = true,
                    EstArchive = true
                };

                _db.ProcesVerbaux.Add(pv);
            }
            else
            {
                pv.Contenu = contenu.ToString();
                pv.DateGeneration = DateTime.UtcNow;
                pv.UrlPdf = $"uploads/documents/pv/{fileName}";
            }

            await _db.SaveChangesAsync();
        }

        // =========================
        // 📥 TÉLÉCHARGEMENT PDF
        // =========================
        public async Task<(byte[] Content, string FileName)> GetPdfAsync(Guid assembleeId)
        {
            var pv = await _db.ProcesVerbaux
                .FirstOrDefaultAsync(p => p.AssembleeGeneraleId == assembleeId);

            if (pv == null)
                throw new InvalidOperationException("Procès-verbal introuvable.");

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                pv.UrlPdf.Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("PDF introuvable.", fullPath);

            var bytes = await File.ReadAllBytesAsync(fullPath);
            return (bytes, Path.GetFileName(fullPath));
        }
    }
}
