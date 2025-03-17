using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Infrastructure.Notifications;
using Mottu.Api.Models;
using Mottu.Api.UseCases.MotorcycleUseCases;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
	public IActionResult Post(PostMotorcycleRequest request)
	{
		try
		{
			_useCase.Create(request);

			if(!_notificationService.HaveNotifications())
			{
				return Created();
			}

			return UnprocessableEntity(
				title: "Dados inconsistentes",
				detail: _notificationService.GetMessages()
			);
		}
		catch(Exception e)
		{
			_logger.LogError("Ocorreu um erro inesperado, mensagem: {message}", e.Message);
			return InternalServerError();
		}
	}

	[HttpGet]
	public IActionResult Get([FromQuery] string plate)
	{
		try
		{
			var motorcycles = _useCase.Get(plate);

			return Ok(motorcycles);
		}
		catch(Exception e)
		{
			_logger.LogError("Ocorreu um erro inesperado, mensagem: {message}", e.Message);
			return InternalServerError();
		}
	}

	[HttpPatch]
	public IActionResult Patch([FromRoute] int id, [FromBody] PatchMotorcycleRequest request)
	{
		try
		{
			_useCase.Update(id, request);

			if(_notificationService.HaveNotifications())
			{
				return BadRequest("Dados inválidos", _notificationService.GetMessages());
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
