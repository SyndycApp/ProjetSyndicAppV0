using SyndicApp.Domain.Entities.Annonces;
using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Domain.Entities.Residences;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Residences;

public class Residence : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;

    public ICollection<Lot> Lots { get; set; } = new List<Lot>();
    public ICollection<Employe> EmployesAffectes { get; set; } = new List<Employe>();
    public ICollection<Annonce> Annonces { get; set; } = new List<Annonce>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
