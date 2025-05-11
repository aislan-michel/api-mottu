using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Application.Interfaces;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/entregadores")]
[Produces("application/json")]
public class DeliveryMenController(
	IDeliveryManUseCase useCase,
	ILogger<DeliveryMenController> logger) : ControllerBase
{
	private readonly IDeliveryManUseCase _useCase = useCase;
	private readonly ILogger<DeliveryMenController> _logger = logger;

	[HttpPost]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Post([FromBody] RegisterDeliveryManRequest request)
	{
		//todo: fix this null param
		var result = await _useCase.Register(request);

		if (!result.Success)
		{
			return BadRequest(result);
		}

		return Ok();
	}

	[HttpPatch("{id}/cnh")]
	[Authorize(Roles = "entregador")]
	public IActionResult Patch([FromRoute] string id, [FromBody] PatchDriverLicenseImageRequest request)
	{
		var result = _useCase.Update(id, request);

		if (!result.Success)
		{
			return BadRequest(result);
		}

		return Ok();
	}

	[HttpGet]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Get()
	{
		return Ok(await _useCase.Get());
	}
}