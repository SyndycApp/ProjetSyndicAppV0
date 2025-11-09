using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Residences;

namespace SyndicApp.Application.Interfaces.Residences
{
    public interface IBatimentService
    {
        Task<IReadOnlyList<BatimentDto>> GetByResidenceAsync(Guid residenceId, CancellationToken ct = default);
        Task<BatimentDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<BatimentDto>> GetAllAsync(CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateBatimentDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateBatimentDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);

        Task<Guid?> ResolveIdByNameAsync(string nom, CancellationToken ct = default);
    }
}
