namespace SyndicApp.Application.DTOs.Assemblees;
public record ProcesVerbalVersionDto(
    int NumeroVersion,
    bool EstOfficielle,
    DateTime DateGeneration,
    string UrlPdf
);
