using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees;

public class DashboardAssembleeService : IDashboardAssembleeService
{
    private readonly ApplicationDbContext _db;

    public DashboardAssembleeService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardAgComparatifDto> GetComparatifAsync(Guid residenceId, int annee)
    {
        var assemblees = await _db.AssembleesGenerales
            .Where(a => a.ResidenceId == residenceId && a.Annee == annee)
            .Include(a => a.Resolutions)
            .Include(a => a.Decisions)
            .ToListAsync();

        var nombreAg = assemblees.Count;

        var totalResolutions = assemblees.Sum(a => a.Resolutions.Count);
        var resolutionsAdoptees = assemblees.Sum(a =>
            a.Decisions.Count(d => d.EstAdoptee));

        var tauxAdoption = totalResolutions == 0
            ? 0
            : (decimal)resolutionsAdoptees / totalResolutions * 100;

        var quorumAtteint = assemblees.Count(a =>
            a.Decisions.Any());

        var tauxQuorum = nombreAg == 0
            ? 0
            : (decimal)quorumAtteint / nombreAg * 100;

        var presences = await _db.PresenceAss
            .Where(p => p.AssembleeGenerale.ResidenceId == residenceId &&
                        p.AssembleeGenerale.Annee == annee)
            .ToListAsync();

        var totalTantiemes = presences.Sum(p => p.Tantiemes);
        var participants = presences.Count;

        var tauxParticipation = totalTantiemes == 0
            ? 0
            : (decimal)participants / totalTantiemes * 100;

        return new DashboardAgComparatifDto(
            annee,
            nombreAg,
            tauxParticipation,
            tauxQuorum,
            tauxAdoption
        );
    }

    public async Task<List<ParticipationParAgDto>> GetParticipationParAgAsync(Guid residenceId, int annee)
    {
        return await _db.AssembleesGenerales
            .Where(a => a.ResidenceId == residenceId && a.Annee == annee)
            .Select(a => new ParticipationParAgDto(
                a.Titre,
                _db.PresenceAss
                    .Where(p => p.AssembleeGeneraleId == a.Id)
                    .Sum(p => p.Tantiemes)
            ))
            .ToListAsync();
    }

    public async Task<RepartitionVotesDto> GetRepartitionVotesAsync(Guid assembleeId)
    {
        var votes = await _db.Votes
            .Where(v => v.Resolution.AssembleeGeneraleId == assembleeId)
            .ToListAsync();

        return new RepartitionVotesDto(
            votes.Where(v => v.Choix == Domain.Enums.Assemblees.ChoixVote.Pour)
                .Sum(v => v.PoidsVote),

            votes.Where(v => v.Choix == Domain.Enums.Assemblees.ChoixVote.Contre)
                .Sum(v => v.PoidsVote),

            votes.Where(v => v.Choix == Domain.Enums.Assemblees.ChoixVote.Abstention)
                .Sum(v => v.PoidsVote)
        );
    }
}
