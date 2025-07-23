using SyndicApp.Domain.Entities.Common;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Documents;

public class CategorieDocument : BaseEntity
{
    public string Nom { get; set; } = string.Empty;

    public ICollection<Document> Documents { get; set; } = new List<Document>();
}
