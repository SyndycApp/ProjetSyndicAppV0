using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.Interfaces.Common
{
    public interface IGeocodingService
    {
        Task<(double lat, double lng)?> GeocodeAsync(string address);
    }
}
