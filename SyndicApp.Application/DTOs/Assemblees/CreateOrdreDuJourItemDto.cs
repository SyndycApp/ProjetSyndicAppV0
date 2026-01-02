using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record CreateOrdreDuJourItemDto(
    int Ordre,
    string Titre,
    string? Description
);
    public record OrdreDuJourItemDto(
    Guid Id,
    int Ordre,
    string Titre,
    string? Description
);
}
