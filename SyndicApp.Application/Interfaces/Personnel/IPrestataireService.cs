using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Personnel;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPrestataireService
    {
        Task<IReadOnlyList<PrestataireDto>> GetAllAsync(string? search = null);
        Task<PrestataireDto?> GetByIdAsync(Guid id);
        Task<PrestataireDto> CreateAsync(PrestataireCreateDto dto);
        Task<PrestataireDto?> UpdateAsync(Guid id, PrestataireUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}
