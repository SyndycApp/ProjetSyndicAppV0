using SyndicApp.Domain.Enums.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record PresenceAssDto(
    Guid AssembleeId,
    Guid LotId,
    TypePresence Type
);

    public record CreateProcurationDto(
        Guid AssembleeId,
        Guid MandataireId,
        Guid LotId
    );

}
