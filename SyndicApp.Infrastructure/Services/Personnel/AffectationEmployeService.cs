using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class AffectationEmployeService : IAffectationEmployeService
    {
        private readonly ApplicationDbContext _db;

        public AffectationEmployeService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AffecterAsync(Guid userId, Guid residenceId, string role)
        {
            var userExists = await _db.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new InvalidOperationException("Utilisateur introuvable.");

            var residenceExists = await _db.Residences.AnyAsync(r => r.Id == residenceId);
            if (!residenceExists)
                throw new InvalidOperationException("Résidence introuvable.");

            var active = await _db.EmployeAffectationResidences.AnyAsync(a =>
                a.UserId == userId &&
                a.ResidenceId == residenceId &&
                a.DateFin == null);

            if (active)
                throw new InvalidOperationException("Affectation déjà active.");

            _db.EmployeAffectationResidences.Add(new EmployeAffectationResidence
            {
                UserId = userId,
                ResidenceId = residenceId,
                DateDebut = DateTime.UtcNow,
                RoleSurSite = role
            });

            await _db.SaveChangesAsync();
        }

        public async Task CloturerAsync(Guid affectationId)
        {
            var entity = await _db.EmployeAffectationResidences.FindAsync(affectationId)
                ?? throw new InvalidOperationException("Affectation introuvable.");

            entity.DateFin = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<AffectationEmployeDto>> GetHistoriqueAsync(Guid employeId)
        {
            return await _db.EmployeAffectationResidences
                .Where(a => a.UserId == employeId)
                .OrderByDescending(a => a.DateDebut)
                .Select(a => new AffectationEmployeDto(a))
                .ToListAsync();
        }
    }

}
