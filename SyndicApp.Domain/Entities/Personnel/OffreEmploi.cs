using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Personnel;
using System;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Personnel;

public class OffreEmploi : BaseEntity
{
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DatePublication { get; set; }

    public ICollection<Candidature> Candidatures { get; set; } = new List<Candidature>();
}
