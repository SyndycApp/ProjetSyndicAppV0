using SyndicApp.Domain.Entities.Annonces;
using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Users;
using System;

namespace SyndicApp.Domain.Entities.Annonces;

public class Annonce : BaseEntity
{
    public string Titre { get; set; } = string.Empty;
    public string Contenu { get; set; } = string.Empty;
    public DateTime DatePublication { get; set; }

    public Guid CategorieAnnonceId { get; set; }
    public CategorieAnnonce CategorieAnnonce { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
