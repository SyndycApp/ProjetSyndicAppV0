using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Residences;
using SyndicApp.Domain.Entities.Users;
using System;

namespace SyndicApp.Domain.Entities.LocauxCommerciaux;

public class LocalCommercial : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public Guid LotId { get; set; }
    public Lot Lot { get; set; } = null!;

    public Guid? ProprietaireId { get; set; }
    public User? Proprietaire { get; set; }

    public Guid? LocataireId { get; set; }
    public User? Locataire { get; set; }

    public Guid? ActiviteId { get; set; }
    public virtual ActiviteCommerciale? Activite { get; set; }

    public string? ContratLocationUrl { get; set; }  // fichier PDF, facultatif

}
