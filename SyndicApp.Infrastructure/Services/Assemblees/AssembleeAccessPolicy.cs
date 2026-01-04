using Microsoft.Extensions.Options;
using SyndicApp.Application.Config;
using SyndicApp.Application.Interfaces.Assemblees;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Enums.Assemblees;

namespace SyndicApp.Infrastructure.Services.Assemblees
{
    public class AssembleeAccessPolicy : IAssembleeAccessPolicy
    {
        private readonly AssembleeQuorumOptions _options;

        public AssembleeAccessPolicy(
            IOptions<AssembleeQuorumOptions> options)
        {
            _options = options.Value;
        }

        public bool PeutSupprimer(AssembleeGenerale ag)
        {
            return ag.Statut == StatutAssemblee.Brouillon;
        }

        public bool PeutArchiver(AssembleeGenerale ag)
        {
            return ag.Statut == StatutAssemblee.Cloturee
                || ag.Statut == StatutAssemblee.Annulee;
        }

        public decimal GetTauxQuorumRequis(AssembleeGenerale ag)
        {
            return ag.Type switch
            {
                TypeAssemblee.Ordinaire => _options.Ordinaire,
                TypeAssemblee.Extraordinaire => _options.Extraordinaire,
                _ => 0.5m
            };
        }

        public bool PeutVoter(AssembleeGenerale ag)
        {
            return ag.Statut == StatutAssemblee.Ouverte;
        }

        public bool PeutModifierContenu(AssembleeGenerale ag)
        {
            return ag.Statut == StatutAssemblee.Brouillon;
        }

        public bool EstVerrouillee(AssembleeGenerale ag)
        {
            return ag.Statut == StatutAssemblee.Cloturee
                || ag.Statut == StatutAssemblee.Annulee;
        }


        public bool EstLectureSeule(AssembleeGenerale ag)
        {
            return ag.Statut == StatutAssemblee.Cloturee
                || ag.Statut == StatutAssemblee.Annulee;
        }

        public bool EstDansPlageHoraireVote(AssembleeGenerale ag, DateTime nowUtc)
        {
            return ag.Statut == StatutAssemblee.Ouverte
                && ag.DateDebut <= nowUtc
                && ag.DateFin >= nowUtc;
        }

    }
}
