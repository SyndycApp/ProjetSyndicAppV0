using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Incidents;

namespace SyndicApp.Domain.Entities.Personnel
{
    public class Prestataire : BaseEntity
    {
        public string Nom { get; set; } = string.Empty;         
        public string? TypeService { get; set; }                
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }
        public string? Notes { get; set; }

        public ICollection<Intervention> Interventions { get; set; }
            = new List<Intervention>();
    }
}
