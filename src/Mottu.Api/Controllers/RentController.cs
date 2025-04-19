using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Application.Models;
using Mottu.Api.Application.UseCases.RentUseCases;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/locacao")]
[Produces("application/json")]
public class RentController : ApiControllerBase
{
    private readonly IRentUseCase _useCase;
    private readonly ILogger<RentController> _logger;

    public RentController(
        IRentUseCase useCase,
        ILogger<RentController> logger, 
        INotificationService notificationService) : base(notificationService)
    {
        _useCase = useCase;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Post(PostRentRequest request)
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

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] string id)
    {
        try
		{
			var result = _useCase.GetById(id);

			if (!result.Success)
			{
				return NotFound();
			}

			return Ok(result.Data);
		}
		catch (Exception e)
		{
			_logger.LogError("Ocorreu um erro inesperado, mensagem: {message}", e.Message);
			return InternalServerError();
		}
    }

	[HttpPatch("{id}/devolucao")]
    public IActionResult Update([FromRoute] string id, [FromBody] PatchRentRequest request)
    {
        try
		{
			var result = _useCase.Update(id, request);

			if(!result.Success)
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

	[HttpGet()]
    public IActionResult Get()
    {
        try
		{
			var rents = _useCase.Get();

			return Ok(rents);
		}
		catch (Exception e)
		{
			_logger.LogError("Ocorreu um erro inesperado, mensagem: {message}", e.Message);
			return InternalServerError();
		}
    }
}