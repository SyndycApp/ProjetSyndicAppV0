using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Personnel
{
    public record RhKpiDto(
    double TauxPresence,
    double TauxPonctualite,
    double EvolutionMoisPourcent
);
}
