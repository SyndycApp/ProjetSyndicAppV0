using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Documents;
using System;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Assemblees;

public class AssembleeGenerale : BaseEntity
{
    public DateTime Date { get; set; }
    public string Lieu { get; set; } = string.Empty;
    public bool Cloturee { get; set; }

    public ICollection<Convocation> Convocations { get; set; } = new List<Convocation>();
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    public ICollection<Decision> Decisions { get; set; } = new List<Decision>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}
