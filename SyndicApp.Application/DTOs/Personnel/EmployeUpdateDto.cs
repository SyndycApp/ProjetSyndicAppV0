using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Personnel
{
    public class EmployeUpdateDto
    {
        public string? TypeContrat { get; set; }
        public DateTime? DateDebutContrat { get; set; }
        public DateTime? DateFinContrat { get; set; }

        public List<HoraireDto> Horaires { get; set; } = [];
        public List<string> Missions { get; set; } = [];
        public List<Guid> ResidenceIds { get; set; } = [];
    }

}
