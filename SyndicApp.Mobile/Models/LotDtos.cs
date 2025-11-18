using System;

namespace SyndicApp.Mobile.Models
{
    public class LotDto
    {
        public Guid Id { get; set; }
        public string? NumeroLot { get; set; }
        public string? Type { get; set; }
        public double Surface { get; set; }
        public Guid ResidenceId { get; set; }
        public Guid? BatimentId { get; set; }
        public string? BatimentNom { get; set; }

        // ---- Nouveaux champs pour l’UI ----
        public bool EstOccupe { get; set; }
        public string? OccupantNom { get; set; }

        // Texte prêt à binder dans le XAML
        public string StatutOccupation => EstOccupe ? "Occupé" : "Libre";

        public string OccupantDisplay =>
            EstOccupe
                ? (!string.IsNullOrWhiteSpace(OccupantNom)
                    ? $"Occupant : {OccupantNom}"
                    : "Occupant : (non renseigné)")
                : "Pas d’occupant";
    }

    public class CreateLotDto
    {
        public string? NumeroLot { get; set; }
        public string? Type { get; set; }
        public double Surface { get; set; }
        public Guid ResidenceId { get; set; }
        public Guid? BatimentId { get; set; }
    }

    public class UpdateLotDto : CreateLotDto { }
}
