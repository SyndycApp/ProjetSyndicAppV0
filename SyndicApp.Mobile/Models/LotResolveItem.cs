using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Models
{
    public sealed class LotResolveItem
    {
        public Guid Id { get; set; }
        public string? NumeroLot { get; set; }
        public string? Type { get; set; }
    }
}
