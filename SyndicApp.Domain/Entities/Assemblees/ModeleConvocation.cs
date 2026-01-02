using SyndicApp.Domain.Entities.Common;


namespace SyndicApp.Domain.Entities.Assemblees
{
    public class ModeleConvocation : BaseEntity
    {
        public string Nom { get; set; } = null!;
        public string Contenu { get; set; } = null!;
        public bool EstParDefaut { get; set; }
    }
}
