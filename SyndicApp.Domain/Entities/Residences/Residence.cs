using SyndicApp.Domain.Entities.Annonces;
using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Domain.Entities.Residences;
using SyndicApp.Domain.Entities.Finances;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Residences;

public class Residence : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double RayonAutoriseMetres { get; set; } = 100;

    // --- Structure ---
    public ICollection<Batiment> Batiments { get; set; } = new List<Batiment>();
    public ICollection<Lot> Lots { get; set; } = new List<Lot>();

    // --- Finances ---
    public ICollection<Charge> Charges { get; set; } = new List<Charge>();

    // --- Incidents / Devis / Interventions (Sprint 6) ---
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    public ICollection<DevisTravaux> DevisTravaux { get; set; } = new List<DevisTravaux>();
    public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();

    // --- Documents & Annonces ---
    public ICollection<Document> Documents { get; set; } = new List<Document>();
    public ICollection<Annonce> Annonces { get; set; } = new List<Annonce>();
}
