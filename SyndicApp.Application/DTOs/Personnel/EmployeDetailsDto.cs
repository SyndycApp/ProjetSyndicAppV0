using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Personnel
{
    public class EmployeDetailsDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string TypeContrat { get; set; } = string.Empty;
        public DateTime DateDebutContrat { get; set; }
        public DateTime? DateFinContrat { get; set; }

        public List<HoraireDto> Horaires { get; set; } = [];
        public List<string> Missions { get; set; } = [];
        public List<string> Residences { get; set; } = [];
    }

}
