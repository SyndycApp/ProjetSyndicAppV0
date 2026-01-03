using SyndicApp.Application.DTOs.Assemblees;


namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IAnnotationAssembleeService
    {
        Task AjouterAsync(Guid assembleeId, Guid syndicId, CreateAnnotationDto dto);
        Task ModifierAsync(Guid annotationId, Guid syndicId, CreateAnnotationDto dto);
        Task SupprimerAsync(Guid annotationId, Guid syndicId);
        Task<List<AnnotationDto>> GetByAssembleeAsync(Guid assembleeId);
    }

}
