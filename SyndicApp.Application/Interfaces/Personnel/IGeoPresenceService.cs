using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Personnel
{
    public interface IGeoPresenceService
    {
        bool IsWithinRadius(
            double userLat,
            double userLng,
            double residenceLat,
            double residenceLng,
            double radiusMeters);
    }
}
