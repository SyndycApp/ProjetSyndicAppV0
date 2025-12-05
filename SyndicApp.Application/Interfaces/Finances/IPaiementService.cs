using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Finances;

namespace SyndicApp.Application.Interfaces.Finances
{
 public interface IPaiementService
    {
        Task<IReadOnlyList<PaiementDto>> GetAllAsync(CancellationToken ct = default);
        Task<PaiementDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(CreatePaiementDto dto, CancellationToken ct = default); // validations anti-surpaiement
        Task<List<PaiementDto>> GetByAppelIdAsync(Guid appelId);
    }
}