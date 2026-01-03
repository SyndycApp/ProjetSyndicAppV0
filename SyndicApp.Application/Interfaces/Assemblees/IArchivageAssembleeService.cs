using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IArchivageAssembleeService
    {
        Task ArchiverAsync(Guid assembleeId, Guid syndicId);
        Task SupprimerAsync(Guid assembleeId, Guid syndicId);
    }
}
