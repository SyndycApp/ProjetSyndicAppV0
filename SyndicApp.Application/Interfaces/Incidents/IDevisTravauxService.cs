using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Devis;

namespace SyndicApp.Application.Interfaces.Incidents
{
    public interface IDevisTravauxService
    {
        Task<DevisDto?> CreateAsync(DevisCreateDto dto);
        Task<DevisDto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<DevisDto?>> GetAllAsync(int page = 1, int pageSize = 50);
        Task<IReadOnlyList<DevisDto?>> GetByResidenceAsync(Guid residenceId, int page = 1, int pageSize = 20);
        Task<DevisDto?> DecideAsync(Guid devisId, DevisDecisionDto dto); // Accepter/Refuser/Terminer/TravauxEnCours
        Task DeleteAsync(Guid id); // interdit si Terminé
    }
}
