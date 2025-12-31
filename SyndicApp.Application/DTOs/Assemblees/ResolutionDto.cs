using SyndicApp.Domain.Enums.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record CreateResolutionDto(
    int Numero,
    string Titre,
    string Description
);

    public record ResolutionDto(
        Guid Id,
        int Numero,
        string Titre,
        string Description,
        StatutResolution Statut
    );

}
