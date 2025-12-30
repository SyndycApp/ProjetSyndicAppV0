using SyndicApp.Application.DTOs.Personnel;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPrestataireNoteService
    {
        Task AjouterOuMettreAJourAsync(
            Guid auteurSyndicId,
            PrestataireNoteCreateDto dto);

        Task<PrestataireNoteDto> GetMoyenneAsync(Guid prestataireId);
    }
}
