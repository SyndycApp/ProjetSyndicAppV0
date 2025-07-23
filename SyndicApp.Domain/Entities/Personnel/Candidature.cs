using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Personnel;
using System;

namespace SyndicApp.Domain.Entities.Personnel;

public class Candidature : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;

    public string? CvUrl { get; set; }
    public string? LettreMotivationUrl { get; set; }

    public Guid OffreEmploiId { get; set; }
    public OffreEmploi OffreEmploi { get; set; } = null!;
}
