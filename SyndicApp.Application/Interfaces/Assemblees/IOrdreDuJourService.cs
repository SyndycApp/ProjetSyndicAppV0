using SyndicApp.Application.DTOs.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IOrdreDuJourService
    {
        Task AjouterAsync(Guid assembleeId, CreateOrdreDuJourItemDto dto);
        Task<List<OrdreDuJourItemDto>> GetByAssembleeAsync(Guid assembleeId);
        Task SupprimerAsync(Guid ordreDuJourItemId);
    }
}
