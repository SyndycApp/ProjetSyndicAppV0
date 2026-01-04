using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record DecisionDto(
    Guid ResolutionId,
    Guid AssembleeGeneraleId,
    string Titre,
    string Description,
    decimal TotalPour,
    decimal TotalContre,
    decimal TotalAbstention,
    decimal TotalExprime,
    bool EstAdoptee,
    DateTime DateDecision
);

}
