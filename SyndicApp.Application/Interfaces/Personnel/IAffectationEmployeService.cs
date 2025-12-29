using SyndicApp.Application.DTOs.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IAffectationEmployeService
    {
        Task AffecterAsync(Guid UserId, Guid residenceId, string role);
        Task CloturerAsync(Guid affectationId);
        Task<IReadOnlyList<AffectationEmployeDto>> GetHistoriqueAsync(Guid employeId);
    }

}
