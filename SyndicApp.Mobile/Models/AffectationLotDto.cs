using System;

namespace SyndicApp.Mobile.Models
{
    public sealed class AffectationLotDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid LotId { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public bool EstProprietaire { get; set; }

        // Optionnel pour l’UI (si ton API les renvoie)
        public string? UserNom { get; set; }
        public string? LotNumero { get; set; }

        public string Statut => DateFin == null ? "Active" : "Clôturée";
    }

    public sealed class CreateAffectationLotDto
    {
        public Guid UserId { get; set; }
        public Guid LotId { get; set; }
        public DateTime DateDebut { get; set; }
        public bool EstProprietaire { get; set; }
    }

    public sealed class UpdateAffectationLotDto
    {
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public bool EstProprietaire { get; set; }
    }

    public sealed class AffectationClotureDto
    {
        public DateTime DateFin { get; set; }
    }

    public sealed class AffectationChangerStatutDto
    {
        public bool EstProprietaire { get; set; }
    }

    public sealed class AssignerLocataireDto
    {
        public Guid LotId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateDebut { get; set; }
    }

    // ⚠️ Pas de ApiOkDto ici : on utilise celui qui existe déjà chez toi.
}
