using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Residences;

namespace SyndicApp.Application.Interfaces.Residences
{
    public interface ILotService
    {
        Task<IReadOnlyList<LotDto>> GetByResidenceAsync(Guid residenceId, CancellationToken ct = default);
        Task<LotDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<LotDto>> GetAllAsync(CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateLotDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateLotDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
