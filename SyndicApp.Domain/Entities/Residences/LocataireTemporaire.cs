using SyndicApp.Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace SyndicApp.Domain.Entities.Residences;

public class LocataireTemporaire : BaseEntity
{
    public Guid LotId { get; set; }
    public Lot Lot { get; set; } = null!;

    [Required]
    public string Nom { get; set; } = string.Empty;

    [Required]
    public string Prenom { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    [Required]
    public string Telephone { get; set; } = string.Empty;

    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
}
