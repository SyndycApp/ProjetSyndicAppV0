using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Domain.Entities.Residences;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Personnel;

public class Employe : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Poste { get; set; } = string.Empty;

    public ICollection<Residence> ResidencesAffectees { get; set; } = new List<Residence>();
    public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
}
