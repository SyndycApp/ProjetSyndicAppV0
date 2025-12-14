using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Communication
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(
            Stream stream,
            string fileName,
            string folder,
            string contentType
        );
    }

}
