using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Application.DTOs.Assemblees
{
    public record AjouterCommentairePvDto(
    Guid ProcesVerbalVersionId,
    string Commentaire
);

    public record VerifierIntegritePvDto(
    Guid ProcesVerbalVersionId,
    bool EstValide,
    bool PdfValide,
    bool ContenuValide,
    string HashStocke,
    string HashCalcule,
    DateTime DateVerification
);

}
