using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record ProcurationViewDto(
     string Donneur,
     string Mandataire,
     decimal Tantiemes
 );

}
