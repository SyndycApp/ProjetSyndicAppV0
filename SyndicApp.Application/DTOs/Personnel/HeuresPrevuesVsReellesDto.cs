using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Personnel
{
    public record RhDashboardDetailsDto
   (
       HeuresPrevuesVsReellesDto Heures,
       RetardStatsDto Retards,
       int AbsencesNonJustifiees
   );
    public record HeuresPrevuesVsReellesDto(
    double HeuresPrevues,
    double HeuresReelles
);
    public record RetardStatsDto(
    int NombreRetards,
    double DureeTotaleHeures
);
    public record ScoreMensuelDto(
      int Annee,
      int Mois,
      int Score
  );
}
