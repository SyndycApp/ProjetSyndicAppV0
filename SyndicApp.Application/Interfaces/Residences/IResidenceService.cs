using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Residences;

namespace SyndicApp.Application.Interfaces.Residences
{
    public interface IResidenceService
    {
        Task<IReadOnlyList<ResidenceDto>> GetAllAsync(CancellationToken ct = default);
        Task<ResidenceDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<Guid> CreateAsync(CreateResidenceDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateResidenceDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<LotDto>> GetLotsAsync(Guid residenceId, CancellationToken ct = default);
        Task<IReadOnlyList<ResidenceOccupantDto>> GetOccupantsAsync(Guid residenceId, CancellationToken ct = default);

        Task<ResidenceDetailsDto?> GetResidenceDetailsAsync(Guid residenceId, CancellationToken ct = default);

        Task<Guid?> LookupIdByNameAsync(string name, CancellationToken ct = default);
    }
}
