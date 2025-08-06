using System;

namespace SyndicApp.Application.DTOs.Residences
{
    public class CreateAffectationLotDto
    {
        public Guid LotId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateDebut { get; set; }
        public bool EstProprietaire { get; set; }
    }
}
