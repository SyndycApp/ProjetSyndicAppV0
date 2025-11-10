using Refit;
using SyndicApp.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Api
{
    // ALT = singulier "AffectationLots"
    public interface IAffectationLotsApiAlt
    {
        [Get("/api/AffectationLots")]
        Task<List<AffectationLotDto>> GetAllAsync();

        [Get("/api/AffectationLots/{id}")]
        Task<AffectationLotDto> GetByIdAsync(Guid id);

        [Get("/api/AffectationLots/by-lot/{lotId}")]
        Task<List<AffectationLotDto>> GetByLotAsync(Guid lotId);

        [Get("/api/AffectationLots/by-user/{userId}")]
        Task<List<AffectationLotDto>> GetByUserAsync(Guid userId);

        [Get("/api/AffectationLots/lot/{lotId}/historique")]
        Task<List<AffectationLotDto>> GetHistoriqueByLotAsync(Guid lotId);

        [Get("/api/AffectationLots/lot/{lotId}/occupant-actuel")]
        Task<AffectationLotDto?> GetOccupantActuelAsync(Guid lotId);

        [Post("/api/AffectationLots")]
        Task<AffectationLotDto> CreateAsync([Body] CreateAffectationLotDto dto);

        [Put("/api/AffectationLots/{id}")]
        Task<AffectationLotDto> UpdateAsync(Guid id, [Body] UpdateAffectationLotDto dto);

        [Delete("/api/AffectationLots/{id}")]
        Task<ApiOkDto> DeleteAsync(Guid id);

        [Put("/api/AffectationLots/{id}/cloturer")]
        Task<AffectationLotDto> CloturerAsync(Guid id, [Body] AffectationClotureDto dto);

        [Put("/api/AffectationLots/{id}/changer-statut")]
        Task<AffectationLotDto> ChangerStatutAsync(Guid id, [Body] AffectationChangerStatutDto dto);

        [Post("/api/AffectationLots/assigner-locataire")]
        Task<AffectationLotDto> AssignerLocataireAsync([Body] AssignerLocataireDto dto);
    }
}
