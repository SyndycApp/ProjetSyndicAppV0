using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PrestataireNoteService : IPrestataireNoteService
    {
        private readonly ApplicationDbContext _db;

        public PrestataireNoteService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AjouterOuMettreAJourAsync(
            Guid auteurSyndicId,
            PrestataireNoteCreateDto dto)
        {
            var note = await _db.PrestataireNotes
                .FirstOrDefaultAsync(n =>
                    n.PrestataireId == dto.PrestataireId &&
                    n.AuteurSyndicId == auteurSyndicId);

            if (note == null)
            {
                note = new PrestataireNote
                {
                    PrestataireId = dto.PrestataireId,
                    AuteurSyndicId = auteurSyndicId
                };

                _db.PrestataireNotes.Add(note);
            }

            note.Qualite = dto.Qualite;
            note.Delai = dto.Delai;
            note.Communication = dto.Communication;

            await _db.SaveChangesAsync();
        }

        public async Task<PrestataireNoteDto> GetMoyenneAsync(Guid prestataireId)
        {
            var notes = await _db.PrestataireNotes
                .AsNoTracking()
                .Where(n => n.PrestataireId == prestataireId)
                .ToListAsync();

            if (!notes.Any())
            {
                return new PrestataireNoteDto
                {
                    Moyenne = 0,
                    NbNotes = 0
                };
            }

            var moyenne = notes.Average(n =>
                (n.Qualite + n.Delai + n.Communication) / 3.0);

            return new PrestataireNoteDto
            {
                Moyenne = Math.Round(moyenne, 2),
                NbNotes = notes.Count
            };
        }
    }
}
