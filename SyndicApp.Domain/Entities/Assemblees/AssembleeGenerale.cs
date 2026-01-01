using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Residences;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Domain.Entities.Assemblees;

public class AssembleeGenerale : BaseEntity
{
    public string Titre { get; set; } = string.Empty;

    public TypeAssemblee Type { get; set; }
    public StatutAssemblee Statut { get; set; } = StatutAssemblee.Brouillon;

    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }

    public int Annee { get; set; }

    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;

    public Guid CreeParId { get; set; }

    public DateTime? DateCloture { get; set; }
    public bool EstArchivee { get; set; }

    public ICollection<Resolution> Resolutions { get; set; } = new List<Resolution>();
    public ICollection<Convocation> Convocations { get; set; } = new List<Convocation>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();

    public ICollection<Decision> Decisions { get; set; } = new List<Decision>();
    public ProcesVerbal? ProcesVerbal { get; set; }
}
