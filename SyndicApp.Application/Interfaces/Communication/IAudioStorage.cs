using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Communication
{
    public interface IAudioStorage
    {
        Task<string> SaveAsync(
            Stream audioStream,
            string fileName,
            string contentType
        );
    }
}
