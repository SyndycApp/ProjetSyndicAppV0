using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class UpdateAffectationLotDto
    {
        public Guid Id { get; set; }
        public Guid LotId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateDebut { get; set; }
        public bool EstProprietaire { get; set; }
    }
}
