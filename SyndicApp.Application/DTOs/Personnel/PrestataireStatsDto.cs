using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Personnel
{
    public record PrestataireStatsDto(
    int NbInterventions,
    double DelaiMoyenJours,
    decimal CoutTotal
);
}
