using Refit;
using SyndicApp.Mobile.Models; // <- ton ApiOkDto est ici
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Api
{
    public interface IAffectationsLotsApi
    {
        [Get("/api/AffectationsLots")]
        Task<List<AffectationLotDto>> GetAllAsync();

        [Get("/api/AffectationsLots/{id}")]
        Task<AffectationLotDto> GetByIdAsync(Guid id);

        [Get("/api/AffectationsLots/by-lot/{lotId}")]
        Task<List<AffectationLotDto>> GetByLotAsync(Guid lotId);

        [Get("/api/AffectationsLots/by-user/{userId}")]
        Task<List<AffectationLotDto>> GetByUserAsync(Guid userId);

        [Get("/api/AffectationsLots/lot/{lotId}/historique")]
        Task<List<AffectationLotDto>> GetHistoriqueByLotAsync(Guid lotId);

        [Get("/api/AffectationsLots/lot/{lotId}/occupant-actuel")]
        Task<AffectationLotDto?> GetOccupantActuelAsync(Guid lotId);

        [Post("/api/AffectationsLots")]
        Task<AffectationLotDto> CreateAsync([Body] CreateAffectationLotDto dto);

        [Put("/api/AffectationsLots/{id}")]
        Task<AffectationLotDto> UpdateAsync(Guid id, [Body] UpdateAffectationLotDto dto);

        [Delete("/api/AffectationsLots/{id}")]
        Task<ApiOkDto> DeleteAsync(Guid id); // ← TON ApiOkDto

        [Put("/api/AffectationsLots/{id}/cloturer")]
        Task<AffectationLotDto> CloturerAsync(Guid id, [Body] AffectationClotureDto dto);

        [Put("/api/AffectationsLots/{id}/changer-statut")]
        Task<AffectationLotDto> ChangerStatutAsync(Guid id, [Body] AffectationChangerStatutDto dto);

        [Post("/api/AffectationsLots/assigner-locataire")]
        Task<AffectationLotDto> AssignerLocataireAsync([Body] AssignerLocataireDto dto);
    }
}
