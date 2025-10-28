using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Finances;

namespace SyndicApp.Application.Interfaces.Finances
{
 public interface IAppelDeFondsService
    {
        Task<IReadOnlyList<AppelDeFondsDto>> GetAllAsync(CancellationToken ct = default);
        Task<AppelDeFondsDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateAppelDeFondsDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateAppelDeFondsDto dto, CancellationToken ct = default);
        Task<bool> CloturerAsync(Guid id, CancellationToken ct = default); // verrouille l’appel

        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}