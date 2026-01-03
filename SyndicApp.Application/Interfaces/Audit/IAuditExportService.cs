using SyndicApp.Application.DTOs.Audit;


namespace SyndicApp.Application.Interfaces.Audit
{

    public interface IAuditExportService
    {
        Task<(byte[] Content, string FileName)> ExportCsvAsync(
            AuditLogExportFilterDto filter);

        Task<(byte[] Content, string FileName)> ExportPdfAsync(
            AuditLogExportFilterDto filter);
    }
}
