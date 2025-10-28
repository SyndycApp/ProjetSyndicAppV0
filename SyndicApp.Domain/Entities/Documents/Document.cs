using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Residences;
using SyndicApp.Domain.Entities.Assemblees;
using System;

namespace SyndicApp.Domain.Entities.Documents;

public class Document : BaseEntity
{

    public string Nom { get; set; } = string.Empty;
    public string CheminFichier { get; set; } = string.Empty;
    public DateTime DateAjout { get; set; }

    public Guid? AssembleeGeneraleId { get; set; }       
    public AssembleeGenerale? AssembleeGenerale { get; set; }

    public Guid? CategorieId { get; set; }
    public virtual CategorieDocument? Categorie { get; set; }

    public Guid? ResidenceId { get; set; }
    public Residence? Residence { get; set; }

    public Guid? AjouteParId { get; set; }
}
