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

		return result.Success ? Created() : BadRequest(result);
	}

	[HttpGet]
	[Authorize(Roles = "admin,entregador")]
	public async Task<IActionResult> Get([FromQuery] GetMotorcyclesRequest request)
	{
		_logger.LogInformation("Iniciando busca de motos...");

		return Ok(await _useCase.Get(request.Plate));
	}

	[HttpGet("{id}")]
	[Authorize(Roles = "admin,entregador")]
	public IActionResult GetById([FromRoute] string id)
	{
		var motorcycle = _useCase.Get(id);

        return motorcycle == null ? NotFound() : Ok(motorcycle);
    }

    [HttpPatch("{id}/placa")]
	[Authorize(Roles = "admin")]
	public IActionResult Patch([FromRoute] string id, [FromBody] PatchMotorcycleRequest request)
	{
		var result = _useCase.Update(id, request);

		return result.Success ? Ok() : BadRequest(result);
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "admin")]
	public IActionResult Delete([FromRoute] string id)
	{
		var result = _useCase.Delete(id);

		return result.Success ? Ok() : BadRequest(result);
	}
}