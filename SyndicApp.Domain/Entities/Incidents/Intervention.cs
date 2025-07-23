using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Domain.Entities.Residences;
using System;

namespace SyndicApp.Domain.Entities.Incidents;

public class Intervention : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public DateTime DateIntervention { get; set; }
    public bool EstEffectuee { get; set; }

    public Guid? IncidentId { get; set; }
    public Incident? Incident { get; set; }

    public Guid? ResidenceId { get; set; }
    public Residence? Residence { get; set; }

    public Guid? EmployeId { get; set; }
    public Employe? Employe { get; set; }
}
