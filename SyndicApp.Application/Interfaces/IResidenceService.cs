using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Residences;



namespace SyndicApp.Application.Interfaces
{
	public interface IResidenceService
	{
		// Résidences
		Task<Result<ResidenceDto>> CreateResidenceAsync(CreateResidenceDto dto);
		Task<Result<ResidenceDto>> UpdateResidenceAsync(UpdateResidenceDto dto);
		Task<Result<bool>> DeleteResidenceAsync(Guid id);
		Task<Result<ResidenceDto>> GetResidenceByIdAsync(Guid id);
		Task<Result<List<ResidenceDto>>> GetAllResidencesAsync();

		// Bâtiments
		Task<Result<BatimentDto>> CreateBatimentAsync(CreateBatimentDto dto);
		Task<Result<BatimentDto>> UpdateBatimentAsync(UpdateBatimentDto dto);
		Task<Result<bool>> DeleteBatimentAsync(Guid id);
		Task<Result<List<BatimentDto>>> GetBatimentsByResidenceIdAsync(Guid residenceId);

		// Lots
		Task<Result<LotDto>> CreateLotAsync(CreateLotDto dto);
		Task<Result<LotDto>> UpdateLotAsync(UpdateLotDto dto);
		Task<Result<bool>> DeleteLotAsync(Guid id);
		Task<Result<List<LotDto>>> GetLotsByResidenceIdAsync(Guid residenceId);
		Task<Result<LotDto>> GetLotByIdAsync(Guid id);

		// Affectations
		Task<Result<AffectationLotDto>> CreateAffectationAsync(CreateAffectationLotDto dto);
		Task<Result<AffectationLotDto>> UpdateAffectationAsync(UpdateAffectationLotDto dto);
		Task<Result<bool>> DeleteAffectationAsync(Guid id);
		Task<Result<List<AffectationLotDto>>> GetAffectationsByLotIdAsync(Guid lotId);
		Task<Result<List<AffectationLotDto>>> GetAffectationsByUserIdAsync(Guid userId);

		// Locataires temporaires
		Task<Result<LocataireTemporaireDto>> CreateLocataireTemporaireAsync(CreateLocataireTemporaireDto dto);
		Task<Result<LocataireTemporaireDto>> UpdateLocataireTemporaireAsync(UpdateLocataireTemporaireDto dto);
		Task<Result<bool>> DeleteLocataireTemporaireAsync(Guid id);
		Task<Result<List<LocataireTemporaireDto>>> GetLocatairesTemporairesByLotIdAsync(Guid lotId);


	}
}
