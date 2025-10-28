using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SyndicApp.Application.DTOs.Finances;

namespace SyndicApp.Application.Interfaces.Finances
{

	public interface ISoldeService
	{
		Task<SoldeLotDto> GetSoldeLotAsync(Guid lotId, CancellationToken ct = default);
		Task<SoldeResidenceDto> GetSoldeResidenceAsync(Guid residenceId, CancellationToken ct = default);
	}
}