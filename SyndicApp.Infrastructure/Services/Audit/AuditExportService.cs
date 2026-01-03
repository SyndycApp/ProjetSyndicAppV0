using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SyndicApp.Application.DTOs.Audit;
using SyndicApp.Application.Interfaces.Audit;
using SyndicApp.Infrastructure;
using System.Text;

public class AuditExportService : IAuditExportService
{
    private readonly ApplicationDbContext _db;

    public AuditExportService(ApplicationDbContext db)
    {
        _db = db;
    }

    // =========================
    // 📤 EXPORT CSV
    // =========================
    public async Task<(byte[] Content, string FileName)> ExportCsvAsync(
        AuditLogExportFilterDto filter)
    {
        var logs = await BuildQuery(filter).ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("DateAction;Action;Cible;Auteur");

        foreach (var l in logs)
        {
            sb.AppendLine(
                $"{l.DateAction:yyyy-MM-dd HH:mm:ss};{l.Action};{l.Cible};{l.Auteur}"
            );
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var fileName = $"AuditLogs_{DateTime.UtcNow:yyyyMMdd_HHmm}.csv";

        return (bytes, fileName);
    }

    // =========================
    // 📄 EXPORT PDF
    // =========================
    public async Task<(byte[] Content, string FileName)> ExportPdfAsync(
        AuditLogExportFilterDto filter)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var logs = await BuildQuery(filter).ToListAsync();

        var stream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header()
                    .Text("Journal d’audit — SyndicApp")
                    .FontSize(18)
                    .Bold()
                    .AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn(2);
                        c.RelativeColumn(2);
                        c.RelativeColumn(3);
                        c.RelativeColumn(2);
                    });

                    table.Header(h =>
                    {
                        h.Cell().Text("Date").Bold();
                        h.Cell().Text("Action").Bold();
                        h.Cell().Text("Cible").Bold();
                        h.Cell().Text("Auteur").Bold();
                    });

                    foreach (var l in logs)
                    {
                        table.Cell().Text(l.DateAction.ToString("dd/MM/yyyy HH:mm"));
                        table.Cell().Text(l.Action);
                        table.Cell().Text(l.Cible);
                        table.Cell().Text(l.Auteur);
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text($"Export généré le {DateTime.UtcNow:dd/MM/yyyy HH:mm}");
            });
        }).GeneratePdf(stream);

        var fileName = $"AuditLogs_{DateTime.UtcNow:yyyyMMdd_HHmm}.pdf";
        return (stream.ToArray(), fileName);
    }

    // =========================
    // 🔎 QUERY COMMUNE
    // =========================
    private IQueryable<AuditLogDto> BuildQuery(AuditLogExportFilterDto filter)
    {
        var query = _db.AuditLogs
            .Join(
                _db.Users,
                log => log.UserId,
                user => user.Id,
                (log, user) => new AuditLogDto(
                    log.DateAction,
                    log.Action,
                    log.Cible,
                    user.FullName
                ))
            .AsQueryable();

        if (filter.From.HasValue)
            query = query.Where(l => l.DateAction >= filter.From.Value);

        if (filter.To.HasValue)
            query = query.Where(l => l.DateAction <= filter.To.Value);

        if (filter.AssembleeId.HasValue)
            query = query.Where(l =>
                l.Cible.Contains(filter.AssembleeId.Value.ToString()));

        return query.OrderByDescending(l => l.DateAction);
    }
}
