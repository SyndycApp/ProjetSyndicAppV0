using SyndicApp.Application.DTOs.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPlanningValidationService
    {
        Task ValidateAsync(ValidateMissionDto dto);
        Task<IReadOnlyList<PlanningMissionDto>> GetNonValideesAsync(Guid residenceId, DateOnly date);
    }
}
