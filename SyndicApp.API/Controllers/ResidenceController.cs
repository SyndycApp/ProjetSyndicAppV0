using Microsoft.AspNetCore.Mvc;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace SyndicApp.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ResidenceController : ControllerBase
	{
		private readonly IResidenceService _residenceService;

		public ResidenceController(IResidenceService residenceService)
		{
			_residenceService = residenceService;
		}

		// === RESIDENCES ===

		[HttpPost]
		public async Task<IActionResult> CreateResidence([FromBody] CreateResidenceDto dto)
		{
			var result = await _residenceService.CreateResidenceAsync(dto);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateResidence([FromBody] UpdateResidenceDto dto)
		{
			var result = await _residenceService.UpdateResidenceAsync(dto);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteResidence(Guid id)
		{
			var result = await _residenceService.DeleteResidenceAsync(id);
			return result.Success ? Ok() : BadRequest(result.Errors);
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetResidenceById(Guid id)
		{
			var result = await _residenceService.GetResidenceByIdAsync(id);
			return result.Success ? Ok(result.Data) : NotFound(result.Errors);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllResidences()
		{
			var result = await _residenceService.GetAllResidencesAsync();
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		// === LOTS ===

		[HttpPost("lots")]
		public async Task<IActionResult> CreateLot([FromBody] CreateLotDto dto)
		{
			var result = await _residenceService.CreateLotAsync(dto);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		[HttpPut("lots")]
		public async Task<IActionResult> UpdateLot([FromBody] UpdateLotDto dto)
		{
			var result = await _residenceService.UpdateLotAsync(dto);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		[HttpDelete("lots/{id:guid}")]
		public async Task<IActionResult> DeleteLot(Guid id)
		{
			var result = await _residenceService.DeleteLotAsync(id);
			return result.Success ? Ok() : BadRequest(result.Errors);
		}

		[HttpGet("{residenceId:guid}/lots")]
		public async Task<IActionResult> GetLotsByResidence(Guid residenceId)
		{
			var result = await _residenceService.GetLotsByResidenceIdAsync(residenceId);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		[HttpGet("lots/{id:guid}")]
		public async Task<IActionResult> GetLotById(Guid id)
		{
			var result = await _residenceService.GetLotByIdAsync(id);
			return result.Success ? Ok(result.Data) : NotFound(result.Errors);
		}

		// === AFFECTATIONS LOTS ===

		[HttpPost("affectations")]
		public async Task<IActionResult> CreateAffectation([FromBody] CreateAffectationLotDto dto)
		{
			var result = await _residenceService.CreateAffectationAsync(dto);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		[HttpPut("affectations")]
		public async Task<IActionResult> UpdateAffectation([FromBody] UpdateAffectationLotDto dto)
		{
			var result = await _residenceService.UpdateAffectationAsync(dto);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		[HttpDelete("affectations/{id:guid}")]
		public async Task<IActionResult> DeleteAffectation(Guid id)
		{
			var result = await _residenceService.DeleteAffectationAsync(id);
			return result.Success ? Ok() : BadRequest(result.Errors);
		}

		[HttpGet("lots/{lotId:guid}/affectations")]
		public async Task<IActionResult> GetAffectationsByLotId(Guid lotId)
		{
			var result = await _residenceService.GetAffectationsByLotIdAsync(lotId);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		[HttpGet("users/{userId:guid}/affectations")]
		public async Task<IActionResult> GetAffectationsByUserId(Guid userId)
		{
			var result = await _residenceService.GetAffectationsByUserIdAsync(userId);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		// === LOCATAIRES TEMPORAIRES ===

		[HttpPost("locataires")]
		public async Task<IActionResult> CreateLocataireTemporaire([FromBody] CreateLocataireTemporaireDto dto)
		{
			var result = await _residenceService.CreateLocataireTemporaireAsync(dto);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		[HttpPut("locataires")]
		public async Task<IActionResult> UpdateLocataireTemporaire([FromBody] UpdateLocataireTemporaireDto dto)
		{
			var result = await _residenceService.UpdateLocataireTemporaireAsync(dto);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}

		[HttpDelete("locataires/{id:guid}")]
		public async Task<IActionResult> DeleteLocataireTemporaire(Guid id)
		{
			var result = await _residenceService.DeleteLocataireTemporaireAsync(id);
			return result.Success ? Ok() : BadRequest(result.Errors);
		}

		[HttpGet("lots/{lotId:guid}/locataires")]
		public async Task<IActionResult> GetLocatairesTemporairesByLotId(Guid lotId)
		{
			var result = await _residenceService.GetLocatairesTemporairesByLotIdAsync(lotId);
			return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
		}
	}
}
