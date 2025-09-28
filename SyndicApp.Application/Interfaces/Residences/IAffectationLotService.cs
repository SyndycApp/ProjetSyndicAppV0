using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Residences;

namespace SyndicApp.Application.Interfaces.Residences
{
    public interface IAffectationLotService
    {
        Task<AffectationLotDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<AffectationLotDto>> GetAllAsync(CancellationToken ct = default);
        Task<IReadOnlyList<AffectationHistoriqueDto>> GetHistoriqueByLotAsync(Guid lotId, CancellationToken ct = default);
        Task<IReadOnlyList<AffectationLotDto>> GetByLotAsync(Guid lotId, CancellationToken ct = default);
        Task<IReadOnlyList<AffectationLotDto>> GetByUserAsync(Guid userId, CancellationToken ct = default);

        Task<Guid> CreateAsync(CreateAffectationLotDto dto, CancellationToken ct = default);
        Task<bool> CloturerAsync(Guid id, DateTime dateFin, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateAffectationLotDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
        Task<AffectationHistoriqueDto?> GetOccupantActuelAsync(Guid lotId, CancellationToken ct = default);
    }
}
