using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Application.UseCases.Interfaces;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/motos")]
[Produces("application/json")]
[Authorize(Roles = "admin")]
public class MotorcyclesController(
    IMotorcycleUseCase useCase,
    ILogger<MotorcyclesController> logger) : ApiControllerBase
{
	private readonly IMotorcycleUseCase _useCase = useCase;
	private readonly ILogger<MotorcyclesController> _logger = logger;

    [HttpPost]
	public IActionResult Post([FromBody] PostMotorcycleRequest request)
	{
		try
		{
			var result = _useCase.Create(request);

			//todo: abstract this
			if (!result.Success)
			{
				return BadRequest(result.GetMessages());
			}

			return Created();
		}
		catch (Exception e)
		{
			_logger.LogError("Ocorreu um erro inesperado, mensagem: {message}", e.Message);
			return InternalServerError();
		}
	}

	[HttpGet]
	public IActionResult Get([FromQuery] GetMotorcyclesRequest request)
	{
		try
		{
			_logger.LogInformation("Iniciando busca de motos...");

			var motorcycles = _useCase.Get(request.Plate);

			return Ok(motorcycles);
		}
		catch (Exception e)
		{
			_logger.LogError("Ocorreu um erro inesperado, mensagem: {message}", e.Message);
			return InternalServerError();
		}
	}

	[HttpGet("{id}")]
	public IActionResult GetById([FromRoute] string id)
	{
		try
		{
			var motorcycle = _useCase.Get(id);

			if(motorcycle == null)
			{
				return NotFound();
			}

			return Ok(motorcycle);
		}
		catch (Exception e)
		{
			_logger.LogError("Ocorreu um erro inesperado, mensagem: {message}", e.Message);
			return InternalServerError();
		}
	}

	[HttpPatch("{id}/placa")]
	public IActionResult Patch([FromRoute] string id, [FromBody] PatchMotorcycleRequest request)
	{
		try
		{
			var result =_useCase.Update(id, request);

			if (!result.Success)
			{
				return BadRequest(result.GetMessages());
			}

			return Ok();
		}
		catch (Exception e)
		{
			_logger.LogError("Ocorreu um erro inesperado, mensagem: {message}", e.Message);
			return InternalServerError();
		}
	}

	[HttpDelete("{id}")]
	public IActionResult Delete([FromRoute] string id)
	{
		try
		{
			var result = _useCase.Delete(id);

			if (!result.Success)
			{
				return BadRequest(result.GetMessages());
			}

			return Ok();
		}
		catch (Exception e)
		{
			_logger.LogError("Ocorreu um erro inesperado, mensagem: {message}", e.Message);
			return InternalServerError();
		}
	}
}