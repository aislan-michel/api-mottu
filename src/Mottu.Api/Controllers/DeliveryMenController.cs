using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Application.UseCases.Interfaces;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/entregadores")]
[Produces("application/json")]
public class DeliveryMenController(
    IDeliveryManUseCase useCase,
    ILogger<DeliveryMenController> logger) : ApiControllerBase
{
    private readonly IDeliveryManUseCase _useCase = useCase;
    private readonly ILogger<DeliveryMenController> _logger = logger;

    [HttpPost]
	[Authorize(Roles = "entregador")]
    public IActionResult Post([FromBody] PostDeliveryManRequest request)
    {
        try
		{
			var result = _useCase.Create(request);

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

    [HttpPatch("{id}/cnh")]
	[Authorize(Roles = "entregador")]
    public IActionResult Patch([FromRoute] string id, [FromBody] PatchDriverLicenseImageRequest request)
    {
        try
		{
			var result = _useCase.Update(id, request);

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

	[HttpGet]
	[Authorize(Roles = "admin")]
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