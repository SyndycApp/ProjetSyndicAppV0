using SyndicApp.Application.DTOs.Assemblees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IConvocationService
    {
        Task SendAsync(CreateConvocationDto dto);

        Task RelancerNonLecteursAsync(Guid convocationId);

        Task<List<ConvocationLectureDto>> GetLecteursAsync(Guid convocationId);

        Task EnvoyerEmailsAsync(Guid convocationId);

        Task MarquerCommeLueAsync(Guid convocationId, Guid userId);

        Task AjouterPieceJointeAsync(Guid convocationId, string nomFichier, string urlFichier);
    }

}
