using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IClotureAssembleeService
    {
        Task CloturerAsync(Guid assembleeId, Guid syndicId);
    }
}
