using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Application.Interfaces;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/motos")]
[Produces("application/json")]
public class MotorcyclesController(
	IMotorcycleUseCase useCase,
	ILogger<MotorcyclesController> logger) : ControllerBase
{
	private readonly IMotorcycleUseCase _useCase = useCase;
	private readonly ILogger<MotorcyclesController> _logger = logger;

	[HttpPost]
	[Authorize(Roles = "admin")]
	public IActionResult Post([FromBody] PostMotorcycleRequest request)
	{
		var result = _useCase.Create(request);

		//todo: abstract this
		if (!result.Success)
		{
			return BadRequest(result.GetMessages());
		}

		return Created();
	}

	[HttpGet]
	[Authorize(Roles = "admin,entregador")]
	public async Task<IActionResult> Get([FromQuery] GetMotorcyclesRequest request)
	{
		_logger.LogInformation("Iniciando busca de motos...");

		var motorcycles = await _useCase.Get(request.Plate);

		return Ok(motorcycles);
	}

	[HttpGet("{id}")]
	[Authorize(Roles = "admin,entregador")]
	public IActionResult GetById([FromRoute] string id)
	{
		var motorcycle = _useCase.Get(id);

		if (motorcycle == null)
		{
			return NotFound();
		}

		return Ok(motorcycle);
	}

	[HttpPatch("{id}/placa")]
	[Authorize(Roles = "admin")]
	public IActionResult Patch([FromRoute] string id, [FromBody] PatchMotorcycleRequest request)
	{
		var result = _useCase.Update(id, request);

		if (!result.Success)
		{
			return BadRequest(result.GetMessages());
		}

		return Ok();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "admin")]
	public IActionResult Delete([FromRoute] string id)
	{
		var result = _useCase.Delete(id);

		if (!result.Success)
		{
			return BadRequest(result.GetMessages());
		}

		return Ok();
	}
}