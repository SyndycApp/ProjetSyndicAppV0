// SyndicApp.Infrastructure/Services/Finances/PaiementService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Finances;
using SyndicApp.Application.Interfaces.Finances;

namespace SyndicApp.Infrastructure.Services.Finances
{
    public class PaiementService : IPaiementService
    {
        private readonly ApplicationDbContext _db;
        public PaiementService(ApplicationDbContext db) => _db = db;

        public async Task<List<PaiementDto>> GetByAppelIdAsync(Guid appelId)
        {
            return await _db.Paiements
                .Where(p => p.AppelDeFondsId == appelId)
                .Select(p => new PaiementDto
                {
                    Id = p.Id,
                    Montant = p.Montant,
                    DatePaiement = p.DatePaiement,
                    AppelDeFondsId = p.AppelDeFondsId,
                    UserId = p.UserId,
                    NomCompletUser = _db.Users
                        .Where(u => u.Id == p.UserId)
                        .Select(u => u.FullName)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }


        public async Task<IReadOnlyList<PaiementDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Paiements.AsNoTracking()
                .Select(p => new PaiementDto
                {
                    Id = p.Id,
                    Montant = p.Montant,
                    DatePaiement = p.DatePaiement,
                    AppelDeFondsId = p.AppelDeFondsId,
                    UserId = p.UserId,
                    NomCompletUser = _db.Users
                        .Where(u => u.Id == p.UserId)
                        .Select(u => u.FullName)
                        .FirstOrDefault()
                }).ToListAsync(ct);
        }

        public async Task<PaiementDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Paiements.AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new PaiementDto
                {
                    Id = p.Id,
                    Montant = p.Montant,
                    DatePaiement = p.DatePaiement,
                    AppelDeFondsId = p.AppelDeFondsId,
                    UserId = p.UserId,
                    NomCompletUser = _db.Users
                        .Where(u => u.Id == p.UserId)
                        .Select(u => u.FullName)
                        .FirstOrDefault()
                }).SingleOrDefaultAsync(ct);
        }

        public async Task<Guid> CreateAsync(CreatePaiementDto dto, CancellationToken ct = default)
        {
            if (dto.Montant <= 0) throw new InvalidOperationException("Montant doit être > 0.");

            var appel = await _db.AppelsDeFonds.AsNoTracking().FirstOrDefaultAsync(a => a.Id == dto.AppelDeFondsId, ct);
            if (appel is null) throw new InvalidOperationException("Appel introuvable.");


            var nbLots = await _db.Lots.CountAsync(l => l.ResidenceId == appel.ResidenceId, ct);
            if (nbLots == 0) throw new InvalidOperationException("Aucun lot dans la résidence.");

            var partLot = Math.Round(appel.MontantTotal / nbLots, 2);

            var dejaPaye = await _db.Paiements
                .Where(p => p.AppelDeFondsId == dto.AppelDeFondsId && p.UserId == dto.UserId)
                .SumAsync(p => (decimal?)p.Montant, ct) ?? 0m;

            if (dejaPaye + dto.Montant > partLot + 0.0001m)
                throw new InvalidOperationException($"Paiement dépasserait le dû du lot ({partLot:0.00}). Déjà payé: {dejaPaye:0.00}");

            var entity = new Domain.Entities.Finances.Paiement
            {
                AppelDeFondsId = dto.AppelDeFondsId,
                UserId = dto.UserId,
                Montant = dto.Montant,
                DatePaiement = dto.DatePaiement
            };

            _db.Paiements.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity.Id;
        }
    }
}
