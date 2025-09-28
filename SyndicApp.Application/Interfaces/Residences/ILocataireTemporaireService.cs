using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Residences;

namespace SyndicApp.Application.Interfaces.Residences
{
    public interface ILocataireTemporaireService
    {
        Task<IReadOnlyList<LocataireTemporaireDto>> GetByLotAsync(Guid lotId, CancellationToken ct = default);
        Task<IReadOnlyList<LocataireTemporaireDto>> GetAllAsync(CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateLocataireTemporaireDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateLocataireTemporaireDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
