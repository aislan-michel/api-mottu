using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Application.Models;
using Mottu.Api.Application.UseCases.DeliveryManUseCases;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/entregadores")]
[Produces("application/json")]
public class DeliveryMenController : ApiControllerBase
{
    private readonly IDeliveryManUseCase _useCase;
    private readonly ILogger<DeliveryMenController> _logger;

    public DeliveryMenController(
        IDeliveryManUseCase useCase,
        ILogger<DeliveryMenController> logger, 
        INotificationService notificationService) : base(notificationService)
    {
        _useCase = useCase;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Post([FromBody] PostDeliveryManRequest request)
    {
        try
		{
			_useCase.Create(request);

			if (_notificationService.HaveNotifications())
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

    [HttpPatch("{id}/cnh")]
    public IActionResult Patch([FromRoute] int id, [FromBody] PatchDriverLicenseImageRequest request)
    {
        try
		{
			_useCase.Update(id, request);

			if (_notificationService.HaveNotifications())
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

	[HttpGet]
	public IActionResult Get()
	{
		try
		{
			var deliveryMen = _useCase.Get();

			return Ok(deliveryMen);
		}
		catch (Exception e)
		{
			_logger.LogError("Ocorreu um erro inesperado, mensagem: {message}", e.Message);
			return InternalServerError();
		}
	}
}