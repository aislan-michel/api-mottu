using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Application.Models;
using Mottu.Api.Application.UseCases.MotorcycleUseCases;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/motos")]
[Produces("application/json")]
public class MotorcyclesController : ApiControllerBase
{
	private readonly IMotorcycleUseCase _useCase;
	private readonly ILogger<MotorcyclesController> _logger;

	public MotorcyclesController(
		IMotorcycleUseCase useCase,
		ILogger<MotorcyclesController> logger,
		INotificationService notificationService) : base(notificationService)
	{
		_useCase = useCase;
		_logger = logger;
	}

	[HttpPost]
	public IActionResult Post([FromBody] PostMotorcycleRequest request)
	{
		try
		{
			_useCase.Create(request);

			//todo: abstract this
			if (!_notificationService.HaveNotifications())
			{
				return Created();
			}

			return UnprocessableEntity(
				title: "Dados inconsistentes",
				detail: _notificationService.GetMessages()
			);
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
			_useCase.Update(id, request);

			if (_notificationService.HaveNotifications())
			{
				return BadRequest(_notificationService.GetMessages());
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
			_useCase.Delete(id);

			if (_notificationService.HaveNotifications())
			{
				return BadRequest(_notificationService.GetMessages());
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