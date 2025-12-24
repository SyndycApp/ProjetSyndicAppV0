using System.Collections.Generic;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Personnel;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IPersonnelService
    {
        Task<IReadOnlyList<PersonnelLookupDto>> GetPersonnelInterneAsync();
    }
}
