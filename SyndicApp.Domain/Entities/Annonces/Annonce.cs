using SyndicApp.Domain.Entities.Annonces;
using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Residences;
using System;

namespace SyndicApp.Domain.Entities.Annonces;

public class Annonce : BaseEntity
{
    public string Titre { get; set; } = string.Empty;
    public string Contenu { get; set; } = string.Empty;
    public DateTime DatePublication { get; set; }

    public Guid CategorieId { get; set; }
    public virtual CategorieAnnonce Categorie { get; set; } = null!;
    public Guid? ResidenceId { get; set; }        
    public Residence? Residence { get; set; }
    public Guid UserId { get; set; }
}
