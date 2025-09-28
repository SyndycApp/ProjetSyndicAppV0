using System;
using System.Collections.Generic;

namespace SyndicApp.Application.DTOs.Residences
{
    public class AffectationLotDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid LotId { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public bool EstProprietaire { get; set; }
    }

    public class CreateAffectationLotDto
    {
        public Guid UserId { get; set; }
        public Guid LotId { get; set; }
        public DateTime DateDebut { get; set; }
        public bool EstProprietaire { get; set; }
    }

    public class ClotureAffectationLotDto
    {
        public DateTime DateFin { get; set; }
    }

    public class UpdateAffectationLotDto
    {
        public bool? EstProprietaire { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
    }

    public class AffectationHistoriqueDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid LotId { get; set; }
        public string? NomComplet { get; set; }
        public bool EstProprietaire { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

        public bool EstActive => !DateFin.HasValue || DateFin.Value > DateTime.UtcNow;
    }
}
