namespace SyndicApp.Application.Interfaces.Exports;

public interface IPresenceExportService
{
    Task<byte[]> ExportPdfAsync(Guid employeId, DateOnly from, DateOnly to);
    Task<byte[]> ExportExcelAsync(Guid employeId, DateOnly from, DateOnly to);
}
