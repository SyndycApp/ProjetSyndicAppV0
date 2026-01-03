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

        public async Task AnnulerAsync(Guid assembleeId)
        {
            var ag = await _db.AssembleesGenerales.FindAsync(assembleeId);

            if (ag == null)
                throw new InvalidOperationException("AG introuvable");

            if (ag.Statut == StatutAssemblee.Cloturee)
                throw new InvalidOperationException("AG déjà clôturée");

            ag.Statut = StatutAssemblee.Annulee;
            await _db.SaveChangesAsync();
        }

        public async Task<Guid> DupliquerAsync(Guid assembleeId)
        {
            var ag = await _db.AssembleesGenerales
                .Include(a => a.Resolutions)
                .FirstAsync(a => a.Id == assembleeId);

            var nouvelleAg = new AssembleeGenerale
            {
                Titre = ag.Titre,
                Type = ag.Type,
                ResidenceId = ag.ResidenceId,
                DateDebut = ag.DateDebut.AddYears(1),
                DateFin = ag.DateFin.AddYears(1),
                Annee = ag.Annee + 1,
                Statut = StatutAssemblee.Brouillon
            };

            foreach (var r in ag.Resolutions)
            {
                nouvelleAg.Resolutions.Add(new Resolution
                {
                    Numero = r.Numero,
                    Titre = r.Titre,
                    Description = r.Description
                });
            }

            _db.AssembleesGenerales.Add(nouvelleAg);
            await _db.SaveChangesAsync();

            return nouvelleAg.Id;
        }

        public async Task MettreAJourStatutSiNecessaireAsync(AssembleeGenerale ag)
        {
            var now = DateTime.UtcNow;

            if (ag.Statut == StatutAssemblee.Publiee &&
                ag.DateDebut <= now &&
                ag.DateFin >= now)
            {
                ag.Statut = StatutAssemblee.Ouverte;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<AssembleeDto>> GetHistoriqueAsync(Guid residenceId, AssembleeHistoriqueFilterDto filter)
        {
            var query = _db.AssembleesGenerales
                        .Where(a => a.ResidenceId == residenceId && !a.EstArchivee)
                        .AsQueryable();

            if (filter.Annee.HasValue)
                query = query.Where(a => a.Annee == filter.Annee.Value);

            if (filter.Statut.HasValue)
                query = query.Where(a => a.Statut == filter.Statut.Value);

            if (filter.Type.HasValue)
                query = query.Where(a => a.Type == filter.Type.Value);

            var ags = await query
                .OrderByDescending(a => a.DateDebut)
                .ToListAsync();

            // Mise à jour auto du statut si besoin
            foreach (var ag in ags)
                await MettreAJourStatutSiNecessaireAsync(ag);

            return ags.Select(a => new AssembleeDto(
                a.Id,
                a.Titre,
                a.Type,
                a.Statut,
                a.DateDebut,
                a.DateFin,
                a.Annee))
                .ToList();
        }


        public async Task<List<AssembleeDto>> GetUpcomingAsync(Guid residenceId)
        {
            var ags = await _db.AssembleesGenerales
                     .Where(a => a.ResidenceId == residenceId && !a.EstArchivee && a.Statut != StatutAssemblee.Cloturee && a.Statut != StatutAssemblee.Annulee)
                     .ToListAsync();

            foreach (var ag in ags)
                await MettreAJourStatutSiNecessaireAsync(ag);

            return ags.Select(a => new AssembleeDto(
                a.Id,
                a.Titre,
                a.Type,
                a.Statut,
                a.DateDebut,
                a.DateFin,
                a.Annee))
                .ToList();
        }
    }

}
