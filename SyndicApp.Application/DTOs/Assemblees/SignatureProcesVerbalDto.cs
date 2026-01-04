namespace SyndicApp.Application.DTOs.Assemblees;

public record SignatureProcesVerbalDto(
    Guid UserId,
    string Nom,
    int OrdreSignature,
    bool EstSigne,
    DateTime? DateSignature
);
