using SyndicApp.Domain.Entities.Common;


namespace SyndicApp.Domain.Entities.Assemblees
{
    public class AssembleeRappel : BaseEntity
    {
        public Guid AssembleeGeneraleId { get; set; }
        public int JoursAvant { get; set; } // 15, 7, 1
        public DateTime DateEnvoi { get; set; }
    }
}
