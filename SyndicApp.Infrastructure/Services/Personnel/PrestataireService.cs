using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PrestataireService : IPrestataireService
    {
        private readonly ApplicationDbContext _db;

        public PrestataireService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<PrestataireDto>> GetAllAsync(string? search = null)
        {
            var q = _db.Prestataires
                .AsNoTracking()
                .Include(p => p.Interventions)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(p =>
                    p.Nom.Contains(s) ||
                    (p.TypeService != null && p.TypeService.Contains(s)) ||
                    (p.Email != null && p.Email.Contains(s)));
            }

            return await q
                .OrderBy(p => p.Nom)
                .Select(p => new PrestataireDto
                {
                    Id = p.Id,
                    Nom = p.Nom,
                    TypeService = p.TypeService,
                    Email = p.Email,
                    Telephone = p.Telephone,
                    Adresse = p.Adresse,
                    Notes = p.Notes,
                    NbInterventions = p.Interventions.Count
                })
                .ToListAsync();
        }

        public async Task<PrestataireDto?> GetByIdAsync(Guid id)
        {
            var p = await _db.Prestataires
                .AsNoTracking()
                .Include(p => p.Interventions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (p == null) return null;

            return new PrestataireDto
            {
                Id = p.Id,
                Nom = p.Nom,
                TypeService = p.TypeService,
                Email = p.Email,
                Telephone = p.Telephone,
                Adresse = p.Adresse,
                Notes = p.Notes,
                NbInterventions = p.Interventions.Count
            };
        }

        public async Task<PrestataireDto> CreateAsync(PrestataireCreateDto dto)
        {
            var entity = new Prestataire
            {
                Nom = dto.Nom,
                TypeService = dto.TypeService,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Adresse = dto.Adresse,
                Notes = dto.Notes
            };

            _db.Prestataires.Add(entity);
            await _db.SaveChangesAsync();

            return new PrestataireDto
            {
                Id = entity.Id,
                Nom = entity.Nom,
                TypeService = entity.TypeService,
                Email = entity.Email,
                Telephone = entity.Telephone,
                Adresse = entity.Adresse,
                Notes = entity.Notes,
                NbInterventions = 0
            };
        }

        public async Task<PrestataireDto?> UpdateAsync(Guid id, PrestataireUpdateDto dto)
        {
            var entity = await _db.Prestataires
                .Include(p => p.Interventions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
                return null;

            entity.Nom = dto.Nom;
            entity.TypeService = dto.TypeService;
            entity.Email = dto.Email;
            entity.Telephone = dto.Telephone;
            entity.Adresse = dto.Adresse;
            entity.Notes = dto.Notes;
            entity.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return new PrestataireDto
            {
                Id = entity.Id,
                Nom = entity.Nom,
                TypeService = entity.TypeService,
                Email = entity.Email,
                Telephone = entity.Telephone,
                Adresse = entity.Adresse,
                Notes = entity.Notes,
                NbInterventions = entity.Interventions.Count
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Prestataires
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
                throw new InvalidOperationException("Prestataire introuvable.");

            _db.Prestataires.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
