// SyndicApp.Application/Interfaces/Finances/IChargeService.cs
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Finances;

namespace SyndicApp.Application.Interfaces.Finances
{
    public interface IChargeService
    {
        Task<IReadOnlyList<ChargeDto>> GetAllAsync(CancellationToken ct = default);
        Task<ChargeDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateChargeDto dto, Guid? lotId = null, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateChargeDto dto, Guid? lotId = null, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
