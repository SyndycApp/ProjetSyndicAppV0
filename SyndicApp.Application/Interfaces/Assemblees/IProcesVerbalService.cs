using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IProcesVerbalService
    {
        Task GenerateAsync(Guid assembleeId);
        Task<(byte[] Content, string FileName)> GetPdfAsync(Guid assembleeId);
    }

}
