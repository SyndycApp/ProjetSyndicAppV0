using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record AnnotationDto(
        Guid Id,
        string Contenu,
        DateTime DateCreation,
        DateTime? DateModification
    );

    public record CreateAnnotationDto(string Contenu);
}
