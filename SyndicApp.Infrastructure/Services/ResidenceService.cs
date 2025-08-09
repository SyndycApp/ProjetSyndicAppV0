using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Application.Interfaces;
using SyndicApp.Domain.Entities.Residences;
using SyndicApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SyndicApp.Domain.Entities.Users;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services
{
    public class ResidenceService : IResidenceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ResidenceService> _logger;

        public ResidenceService(ApplicationDbContext context, IMapper mapper, ILogger<ResidenceService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // --- RESIDENCE ---
        public async Task<Result<ResidenceDto>> CreateResidenceAsync(CreateResidenceDto dto)
        {
            try
            {
                var entity = _mapper.Map<Residence>(dto);
                _context.Residences.Add(entity);
                await _context.SaveChangesAsync();
                return Result<ResidenceDto>.Ok(_mapper.Map<ResidenceDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la résidence");
                return Result<ResidenceDto>.Fail("Erreur lors de la création de la résidence.");

            }
        }

        public async Task<Result<ResidenceDto>> UpdateResidenceAsync(UpdateResidenceDto dto)
        {
            try
            {
                var entity = await _context.Residences.FindAsync(dto.Id);
                if (entity == null)
                    return Result<ResidenceDto>.Fail("Résidence introuvable");

                _mapper.Map(dto, entity);
                await _context.SaveChangesAsync();
                return Result<ResidenceDto>.Ok(_mapper.Map<ResidenceDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de la résidence");
                return Result<ResidenceDto>.Fail("Erreur interne");
            }
        }

        public async Task<Result<bool>> DeleteResidenceAsync(Guid id)
        {
            try
            {
                var entity = await _context.Residences.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Fail("Résidence introuvable");

                _context.Residences.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la résidence");
                return Result<bool>.Fail("Erreur interne");
            }
        }

        public async Task<Result<ResidenceDto>> GetResidenceByIdAsync(Guid id)
        {
            try
            {
                var entity = await _context.Residences
                    .Include(r => r.Lots)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (entity == null)
                    return Result<ResidenceDto>.Fail("Résidence introuvable");

                return Result<ResidenceDto>.Ok(_mapper.Map<ResidenceDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la résidence");
                return Result<ResidenceDto>.Fail("Erreur interne");
            }
        }

        public async Task<Result<List<ResidenceDto>>> GetAllResidencesAsync()
        {
            try
            {
                var entities = await _context.Residences.Include(r => r.Lots).ToListAsync();
                return Result<List<ResidenceDto>>.Ok(_mapper.Map<List<ResidenceDto>>(entities));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des résidences");
                return Result<List<ResidenceDto>>.Fail("Erreur interne");
            }
        }

        // --- BATIMENTS --- (TODO)
        public Task<Result<BatimentDto>> CreateBatimentAsync(CreateBatimentDto dto) =>
            Task.FromResult(Result<BatimentDto>.Fail("Non implémenté"));

        public Task<Result<BatimentDto>> UpdateBatimentAsync(UpdateBatimentDto dto) =>
            Task.FromResult(Result<BatimentDto>.Fail("Non implémenté"));

        public Task<Result<bool>> DeleteBatimentAsync(Guid id) =>
            Task.FromResult(Result<bool>.Fail("Non implémenté"));

        public Task<Result<List<BatimentDto>>> GetBatimentsByResidenceIdAsync(Guid residenceId) =>
            Task.FromResult(Result<List<BatimentDto>>.Fail("Non implémenté"));

        // --- LOTS ---
        public async Task<Result<LotDto>> CreateLotAsync(CreateLotDto dto)
        {
            try
            {
                var entity = _mapper.Map<Lot>(dto);
                _context.Lots.Add(entity);
                await _context.SaveChangesAsync();
                return Result<LotDto>.Ok(_mapper.Map<LotDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du lot");
                return Result<LotDto>.Fail("Erreur interne");
            }
        }

        public async Task<Result<LotDto>> UpdateLotAsync(UpdateLotDto dto)
        {
            try
            {
                var entity = await _context.Lots.FindAsync(dto.Id);
                if (entity == null)
                    return Result<LotDto>.Fail("Lot introuvable");

                _mapper.Map(dto, entity);
                await _context.SaveChangesAsync();
                return Result<LotDto>.Ok(_mapper.Map<LotDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du lot");
                return Result<LotDto>.Fail("Erreur interne");
            }
        }

        public async Task<Result<bool>> DeleteLotAsync(Guid id)
        {
            try
            {
                var entity = await _context.Lots.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Fail("Lot introuvable");

                _context.Lots.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du lot");
                return Result<bool>.Fail("Erreur interne");
            }
        }

        public async Task<Result<List<LotDto>>> GetLotsByResidenceIdAsync(Guid residenceId)
        {
            try
            {
                var lots = await _context.Lots
                    .Where(l => l.ResidenceId == residenceId)
                    .ToListAsync();

                return Result<List<LotDto>>.Ok(_mapper.Map<List<LotDto>>(lots));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des lots");
                return Result<List<LotDto>>.Fail("Erreur interne");
            }
        }

        public async Task<Result<LotDto>> GetLotByIdAsync(Guid id)
        {
            try
            {
                var lot = await _context.Lots.FindAsync(id);
                if (lot == null)
                    return Result<LotDto>.Fail("Lot introuvable");

                return Result<LotDto>.Ok(_mapper.Map<LotDto>(lot));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du lot");
                return Result<LotDto>.Fail("Erreur interne");
            }
        }

        // --- LOCATAIRES TEMPORAIRES ---

        public async Task<Result<LocataireTemporaireDto>> CreateLocataireTemporaireAsync(CreateLocataireTemporaireDto dto)
        {
            try
            {
                var entity = _mapper.Map<LocataireTemporaire>(dto);
                _context.LocatairesTemporaires.Add(entity);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<LocataireTemporaireDto>(entity);
                return Result<LocataireTemporaireDto>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création d'un locataire temporaire");
                return Result<LocataireTemporaireDto>.Fail("Erreur interne");
            }
        }

        public async Task<Result<LocataireTemporaireDto>> UpdateLocataireTemporaireAsync(UpdateLocataireTemporaireDto dto)
        {
            try
            {
                var entity = await _context.LocatairesTemporaires.FindAsync(dto.Id);
                if (entity == null)
                    return Result<LocataireTemporaireDto>.Fail("Locataire temporaire introuvable");

                _mapper.Map(dto, entity);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<LocataireTemporaireDto>(entity);
                return Result<LocataireTemporaireDto>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour d'un locataire temporaire");
                return Result<LocataireTemporaireDto>.Fail("Erreur interne");
            }
        }

        public async Task<Result<bool>> DeleteLocataireTemporaireAsync(Guid id)
        {
            try
            {
                var entity = await _context.LocatairesTemporaires.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Fail("Locataire temporaire introuvable");

                _context.LocatairesTemporaires.Remove(entity);
                await _context.SaveChangesAsync();

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression d'un locataire temporaire");
                return Result<bool>.Fail("Erreur interne");
            }
        }

        public async Task<Result<List<LocataireTemporaireDto>>> GetLocatairesTemporairesByLotIdAsync(Guid lotId)
        {
            try
            {
                var entities = await _context.LocatairesTemporaires
                    .Where(l => l.LotId == lotId)
                    .ToListAsync();

                var dtos = _mapper.Map<List<LocataireTemporaireDto>>(entities);
                return Result<List<LocataireTemporaireDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des locataires temporaires par lot");
                return Result<List<LocataireTemporaireDto>>.Fail("Erreur interne");
            }
        }

        // --- AFFECTATIONS --- (voir ton code existant)
        public async Task<Result<AffectationLotDto>> CreateAffectationAsync(CreateAffectationLotDto dto)
        {
            try
            {
                var entity = _mapper.Map<AffectationLot>(dto);
                _context.AffectationsLots.Add(entity);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<AffectationLotDto>(entity);
                return Result<AffectationLotDto>.Ok(resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création d'une affectation");
                return Result<AffectationLotDto>.Fail("Erreur interne");
            }
        }

        public async Task<Result<AffectationLotDto>> UpdateAffectationAsync(UpdateAffectationLotDto dto)
        {
            try
            {
                var entity = await _context.AffectationsLots.FindAsync(dto.Id);
                if (entity == null)
                    return Result<AffectationLotDto>.Fail("Affectation non trouvée");

                _mapper.Map(dto, entity);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<AffectationLotDto>(entity);
                return Result<AffectationLotDto>.Ok(resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour d'une affectation");
                return Result<AffectationLotDto>.Fail("Erreur interne");
            }
        }

        public async Task<Result<bool>> DeleteAffectationAsync(Guid id)
        {
            try
            {
                var entity = await _context.AffectationsLots.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Fail("Affectation non trouvée");

                _context.AffectationsLots.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression d'une affectation");
                return Result<bool>.Fail("Erreur interne");
            }
        }

        public async Task<Result<List<AffectationLotDto>>> GetAffectationsByLotIdAsync(Guid lotId)
        {
            try
            {
                var affectations = await _context.AffectationsLots
                    .Include(a => a.User)
                    .Include(a => a.Lot)
                    .Where(a => a.LotId == lotId)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AffectationLotDto>>(affectations);
                return Result<List<AffectationLotDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des affectations par lot");
                return Result<List<AffectationLotDto>>.Fail("Erreur interne");
            }
        }

        public async Task<Result<List<AffectationLotDto>>> GetAffectationsByUserIdAsync(Guid userId)
        {
            try
            {
                var affectations = await _context.AffectationsLots
                    .Include(a => a.User)
                    .Include(a => a.Lot)
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AffectationLotDto>>(affectations);
                return Result<List<AffectationLotDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des affectations par utilisateur");
                return Result<List<AffectationLotDto>>.Fail("Erreur interne");
            }
        }

    }
}
