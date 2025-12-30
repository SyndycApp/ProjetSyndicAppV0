using ClosedXML.Excel;
using SyndicApp.Application.Interfaces.Exports;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace SyndicApp.Infrastructure.Exports;

public class PresenceExportService : IPresenceExportService
{
    private readonly ApplicationDbContext _db;

    public PresenceExportService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<byte[]> ExportExcelAsync(Guid employeId, DateOnly from, DateOnly to)
    {
        var userId = await _db.Employes
            .Where(e => e.Id == employeId)
            .Select(e => e.UserId)
            .FirstAsync();

        var presences = await _db.Presences
            .Where(p =>
                p.UserId == userId &&
                p.HeureDebut != null &&
                p.HeureDebut >= from.ToDateTime(TimeOnly.MinValue) &&
                p.HeureDebut <= to.ToDateTime(TimeOnly.MaxValue))
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var ws = workbook.AddWorksheet("Présences");

        ws.Cell(1, 1).Value = "Date";
        ws.Cell(1, 2).Value = "Début";
        ws.Cell(1, 3).Value = "Fin";
        ws.Cell(1, 4).Value = "Validée";

        int row = 2;
        foreach (var p in presences)
        {
            ws.Cell(row, 1).Value = p.HeureDebut?.Date;
            ws.Cell(row, 2).Value = p.HeureDebut;
            ws.Cell(row, 3).Value = p.HeureFin;
            ws.Cell(row, 4).Value = p.IsGeoValidated;
            row++;
        }

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);
        return ms.ToArray();
    }

    public async Task<byte[]> ExportPdfAsync(Guid employeId, DateOnly from, DateOnly to)
    {
        // À implémenter plus tard (QuestPDF / iText)
        throw new NotImplementedException("PDF export à implémenter");
    }
}
