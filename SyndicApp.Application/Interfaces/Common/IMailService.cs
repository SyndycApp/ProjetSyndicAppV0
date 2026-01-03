using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Common
{
    public interface IMailService
    {
        Task EnvoyerAsync(
            string email,
            string sujet,
            string contenu,
            bool isHtml = false
        );
    }
}
