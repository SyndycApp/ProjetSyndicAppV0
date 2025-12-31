using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class ProcesVerbalService : IProcesVerbalService
    {
        private readonly ApplicationDbContext _db;

        public ProcesVerbalService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(byte[] Content, string FileName)> GetPdfAsync(Guid assembleeId)
        {
            var pv = await _db.ProcesVerbaux
                .FirstOrDefaultAsync(p => p.AssembleeGeneraleId == assembleeId);

            if (pv == null)
                throw new InvalidOperationException("Procès-verbal introuvable");

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                pv.UrlPdf.Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Fichier PDF introuvable", fullPath);

            var bytes = await File.ReadAllBytesAsync(fullPath);
            return (bytes, Path.GetFileName(fullPath));
        }


        public async Task GenerateAsync(Guid assembleeId)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var uploadsRoot = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "documents",
                "pv"
            );

            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var fileName = $"PV_{assembleeId}.pdf";
            var fullPath = Path.Combine(uploadsRoot, fileName);

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

                        col.Item().Text($"Assemblée ID : {assembleeId}");
                        col.Item().Text($"Date de génération : {DateTime.UtcNow:dd/MM/yyyy HH:mm}");
                        col.Item().LineHorizontal(1);

                        col.Item().Text("Décisions prises :")
                            .Bold();

                        col.Item().Text("- Approbation du budget");
                        col.Item().Text("- Validation des travaux");
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("SyndicApp • Document officiel");
                });
            })
            .GeneratePdf(fullPath);

            var pv = new ProcesVerbal
            {
                AssembleeGeneraleId = assembleeId,
                UrlPdf = $"uploads/documents/pv/{fileName}",
                DateGeneration = DateTime.UtcNow,
                EstArchive = true
            };

            _db.ProcesVerbaux.Add(pv);
            await _db.SaveChangesAsync();
        }

    }

}
