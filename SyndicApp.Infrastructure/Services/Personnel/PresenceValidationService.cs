using Microsoft.EntityFrameworkCore;
using SyndicApp.Application.DTOs.Personnel;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Domain.Entities.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services.Personnel
{
    public class PresenceValidationService : IPresenceValidationService
    {
        private readonly ApplicationDbContext _db;

        public PresenceValidationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task DeclarerAsync(Guid userId, DeclareAbsenceDto dto)
        {
            _db.AbsenceJustifications.Add(new AbsenceJustification
            {
                UserId = userId,
                Date = dto.Date,
                Type = dto.Type,
                Motif = dto.Motif
            });

            await _db.SaveChangesAsync();
        }

        public async Task ValiderAsync(Guid justificationId)
        {
            var j = await _db.AbsenceJustifications.FindAsync(justificationId)
                ?? throw new InvalidOperationException("Justificatif introuvable.");

            j.Validee = true;
            await _db.SaveChangesAsync();
        }


        public async Task<IReadOnlyList<AbsenceJustificationDto>> GetNonValideesAsync()
        {
            return await _db.AbsenceJustifications
                .Where(j => !j.Validee)
                .OrderByDescending(j => j.Date)
                .Select(j => new AbsenceJustificationDto(
                    j.Id,
                    j.UserId,
                    j.Date,
                    j.Type,
                    j.Motif,
                    j.DocumentUrl,
                    j.Validee
                ))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<AbsenceJustificationDto>> GetValideesAsync()
        {
            return await _db.AbsenceJustifications
                .Where(j => j.Validee)
                .OrderByDescending(j => j.Date)
                .Select(j => new AbsenceJustificationDto(
                    j.Id,
                    j.UserId,
                    j.Date,
                    j.Type,
                    j.Motif,
                    j.DocumentUrl,
                    j.Validee
                ))
                .ToListAsync();
        }


    }

}
