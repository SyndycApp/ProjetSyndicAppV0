using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Config
{
    public class PresenceGeoOptions
    {
        public string Mode { get; set; } = "Informative";
        public double ToleranceMetres { get; set; } = 15;
    }
}
