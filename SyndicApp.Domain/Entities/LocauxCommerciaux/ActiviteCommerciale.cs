using SyndicApp.Domain.Entities.Common;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.LocauxCommerciaux;

public class ActiviteCommerciale : BaseEntity
{
    public string TypeActivite { get; set; } = string.Empty; // Exemple : Restaurant, Pressing, Coiffeur...
    public string? Description { get; set; }

    public ICollection<LocalCommercial> Locaux { get; set; } = new List<LocalCommercial>();
}
