using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees;

public class ResolutionService : IResolutionService
{
    private readonly ApplicationDbContext _db;

    public ResolutionService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Guid assembleeId, CreateResolutionDto dto)
    {
        var assemblee = await _db.AssembleesGenerales
            .FirstOrDefaultAsync(a => a.Id == assembleeId);

        if (assemblee == null)
            throw new InvalidOperationException("Assemblée introuvable");

        if (assemblee.Statut != StatutAssemblee.Brouillon)
            throw new InvalidOperationException("Impossible de modifier les résolutions après publication de l’AG");

        var numeroExiste = await _db.Resolutions
            .AnyAsync(r => r.AssembleeGeneraleId == assembleeId && r.Numero == dto.Numero);

        if (numeroExiste)
            throw new InvalidOperationException("Une résolution avec ce numéro existe déjà");

        var resolution = new Resolution
        {
            AssembleeGeneraleId = assembleeId,
            Numero = dto.Numero,
            Titre = dto.Titre,
            Description = dto.Description,
            Statut = StatutResolution.EnAttente
        };

        _db.Resolutions.Add(resolution);
        await _db.SaveChangesAsync();
    }

    public async Task<List<ResolutionDto>> GetByAssembleeAsync(Guid assembleeId)
    {
        return await _db.Resolutions
            .Where(r => r.AssembleeGeneraleId == assembleeId)
            .OrderBy(r => r.Numero)
            .Select(r => new ResolutionDto(
                r.Id,
                r.Numero,
                r.Titre,
                r.Description,
                r.Statut
            ))
            .ToListAsync();
    }
}
