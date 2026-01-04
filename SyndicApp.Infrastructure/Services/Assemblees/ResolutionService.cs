using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees;

public class ResolutionService : IResolutionService
{
    private readonly ApplicationDbContext _db;
    private readonly IAssembleeAccessPolicy _accessPolicy;

    public ResolutionService(ApplicationDbContext db, IAssembleeAccessPolicy accessPolicy)
    {
        _db = db;
        _accessPolicy = accessPolicy;
    }

    public async Task AddAsync(Guid assembleeId, CreateResolutionDto dto)
    {
        var assemblee = await _db.AssembleesGenerales
            .FirstOrDefaultAsync(a => a.Id == assembleeId);

        if (assemblee == null)
            throw new InvalidOperationException("Assemblée introuvable");

        if (_accessPolicy.EstVerrouillee(assemblee))
            throw new InvalidOperationException(
                "Assemblée clôturée : modification interdite."
            );

        if (!_accessPolicy.PeutModifierContenu(assemblee))
            throw new InvalidOperationException(
                "Modification des résolutions interdite à ce stade."
            );


        var numeroExiste = await _db.Resolutions
            .AnyAsync(r => r.AssembleeGeneraleId == assembleeId && r.Numero == dto.Numero);

        if (numeroExiste)
            throw new InvalidOperationException("Une résolution avec ce numéro existe déjà");

        using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            // 1️⃣ Création de la résolution
            var resolution = new Resolution
            {
                AssembleeGeneraleId = assembleeId,
                Numero = dto.Numero,
                Titre = dto.Titre,
                Description = dto.Description,
                Statut = StatutResolution.EnAttente
            };

            _db.Resolutions.Add(resolution);

            // 2️⃣ Création de l’ordre du jour associé
            var odj = new OrdreDuJourItem
            {
                AssembleeGeneraleId = assembleeId,
                Ordre = dto.Numero,              // même ordre que la résolution
                Titre = dto.Titre,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _db.OrdreDuJour.Add(odj);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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
