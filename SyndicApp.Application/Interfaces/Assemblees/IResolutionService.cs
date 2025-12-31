using SyndicApp.Application.DTOs.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IResolutionService
    {
        Task AddAsync(Guid assembleeId, CreateResolutionDto dto);
        Task<List<ResolutionDto>> GetByAssembleeAsync(Guid assembleeId);
    }

}
