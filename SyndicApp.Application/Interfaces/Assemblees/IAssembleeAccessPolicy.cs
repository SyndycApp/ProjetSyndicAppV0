using SyndicApp.Domain.Entities.Assemblees;

namespace SyndicApp.Application.Interfaces.Assemblees
{
    public interface IAssembleeAccessPolicy
    {
        bool PeutVoter(AssembleeGenerale ag);
        bool PeutModifierContenu(AssembleeGenerale ag);
        bool EstLectureSeule(AssembleeGenerale ag);
        bool EstDansPlageHoraireVote(AssembleeGenerale ag, DateTime nowUtc);

        decimal GetTauxQuorumRequis(AssembleeGenerale ag);

        bool PeutArchiver(AssembleeGenerale ag);

        bool PeutSupprimer(AssembleeGenerale ag);

    }
}
