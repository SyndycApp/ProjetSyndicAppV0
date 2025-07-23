using SyndicApp.Domain.Entities.Common;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Annonces;

public class CategorieAnnonce : BaseEntity
{
    public string Nom { get; set; } = string.Empty;

    public ICollection<Annonce> Annonces { get; set; } = new List<Annonce>();
}
