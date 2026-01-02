using SyndicApp.Domain.Entities.Common;

namespace SyndicApp.Domain.Entities.Assemblees
{
    public class ConvocationPieceJointe : BaseEntity
    {
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; } = null!;

        public string NomFichier { get; set; } = null!;
        public string UrlFichier { get; set; } = null!;
    }
}
