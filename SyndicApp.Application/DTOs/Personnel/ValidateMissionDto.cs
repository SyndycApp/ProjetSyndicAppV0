using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Personnel
{
    public record ValidateMissionDto(Guid MissionId, string? Commentaire);

    public record MissionValidationDto
    (
        Guid MissionId,
        bool EstValidee,
        DateTime? DateValidation
    );
}
