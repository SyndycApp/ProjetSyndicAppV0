using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record CreateConvocationDto(
    Guid AssembleeGeneraleId,
    string Contenu,
    List<Guid> DestinataireUserIds
);

}
