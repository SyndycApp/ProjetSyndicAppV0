using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Assemblees;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class AssembleeService : IAssembleeService
    {
        private readonly ApplicationDbContext _db;

        public AssembleeService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> CreateAsync(CreateAssembleeDto dto, Guid userId)
        {
            var ag = new AssembleeGenerale
            {
                Titre = dto.Titre,
                Type = dto.Type,
                DateDebut = dto.DateDebut,
                DateFin = dto.DateFin,
                ResidenceId = dto.ResidenceId,
                Annee = dto.DateDebut.Year,
                CreeParId = userId
            };

            _db.AssembleesGenerales.Add(ag);
            await _db.SaveChangesAsync();

            return ag.Id;
        }

        public async Task PublishAsync(Guid assembleeId)
        {
            var ag = await _db.AssembleesGenerales.FindAsync(assembleeId);
            ag!.Statut = StatutAssemblee.Publiee;
            await _db.SaveChangesAsync();
        }

        public async Task CloseAsync(Guid assembleeId)
        {
            var ag = await _db.AssembleesGenerales
                .Include(a => a.Resolutions)
                .ThenInclude(r => r.Votes)
                .FirstAsync(a => a.Id == assembleeId);

            ag.Statut = StatutAssemblee.Cloturee;
            ag.DateCloture = DateTime.UtcNow;
            ag.EstArchivee = true;

            foreach (var r in ag.Resolutions)
            {
                var totalPour = r.Votes.Where(v => v.Choix == ChoixVote.Pour).Sum(v => v.PoidsVote);
                var totalContre = r.Votes.Where(v => v.Choix == ChoixVote.Contre).Sum(v => v.PoidsVote);

                r.Statut = totalPour > totalContre
                    ? StatutResolution.Adoptee
                    : StatutResolution.Rejetee;
            }

            await _db.SaveChangesAsync();
        }

        public async Task<List<AssembleeDto>> GetUpcomingAsync(Guid residenceId)
        {
            return await _db.AssembleesGenerales
                .Where(a => a.ResidenceId == residenceId && a.Statut != StatutAssemblee.Cloturee)
                .Select(a => new AssembleeDto(
                    a.Id, a.Titre, a.Type, a.Statut, a.DateDebut, a.DateFin, a.Annee))
                .ToListAsync();
        }
    }

}
