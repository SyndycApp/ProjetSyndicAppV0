using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees;

public class OrdreDuJourService : IOrdreDuJourService
{
    private readonly ApplicationDbContext _db;

    public OrdreDuJourService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AjouterAsync(Guid assembleeId, CreateOrdreDuJourItemDto dto)
    {
        var assemblee = await _db.AssembleesGenerales
            .FirstOrDefaultAsync(a => a.Id == assembleeId);

        if (assemblee == null)
            throw new InvalidOperationException("Assemblée introuvable");

        // 🔒 Blocage après publication
        if (assemblee.Statut != StatutAssemblee.Brouillon)
            throw new InvalidOperationException(
                "Impossible de modifier l’ordre du jour après publication");

        var item = new OrdreDuJourItem
        {
            AssembleeGeneraleId = assembleeId,
            Ordre = dto.Ordre,
            Titre = dto.Titre,
            Description = dto.Description
        };

        _db.OrdreDuJour.Add(item);
        await _db.SaveChangesAsync();
    }


    public async Task<List<OrdreDuJourItemDto>> GetByAssembleeAsync(Guid assembleeId)
    {
        return await _db.OrdreDuJour
            .Where(o => o.AssembleeGeneraleId == assembleeId)
            .OrderBy(o => o.Ordre)
            .Select(o => new OrdreDuJourItemDto(
                o.Id,
                o.Ordre,
                o.Titre,
                o.Description
            ))
            .ToListAsync();
    }


    public async Task SupprimerAsync(Guid ordreDuJourItemId)
    {
        var item = await _db.OrdreDuJour
            .Include(o => o.AssembleeGenerale)
            .FirstOrDefaultAsync(o => o.Id == ordreDuJourItemId);

        if (item == null)
            throw new InvalidOperationException("Point d’ordre du jour introuvable");

        if (item.AssembleeGenerale.Statut != StatutAssemblee.Brouillon)
            throw new InvalidOperationException(
                "Impossible de modifier l’ordre du jour après publication");

        _db.OrdreDuJour.Remove(item);
        await _db.SaveChangesAsync();
    }
}
