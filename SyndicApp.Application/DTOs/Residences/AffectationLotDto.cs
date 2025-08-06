using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class AffectationLotDto
    {
        public Guid Id { get; set; }
        public Guid LotId { get; set; }
        public string? NumeroLot { get; set; }

        public Guid UserId { get; set; }
        public string? NomUtilisateur { get; set; }

        public DateTime DateDebut { get; set; }
        public bool EstProprietaire { get; set; }
    }
}
