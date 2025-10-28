using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.Residences;
using System;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Incidents;

public class Incident : BaseEntity
{
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateDeclaration { get; set; }
    public bool EstResolu { get; set; }

    public Guid LotId { get; set; }
    public Lot Lot { get; set; } = null!;

    public Guid DeclareParId { get; set; }

    public Guid? ResidenceId { get; set; }    
    public Residence? Residence { get; set; }

    public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
}
