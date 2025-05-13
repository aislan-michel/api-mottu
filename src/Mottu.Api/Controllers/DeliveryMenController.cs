using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Application.Interfaces;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/entregadores")]
[Produces("application/json")]
public class DeliveryMenController(IDeliveryManUseCase useCase) : ControllerBase
{
	private readonly IDeliveryManUseCase _useCase = useCase;

	[HttpPost]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Post([FromBody] RegisterDeliveryManRequest request)
	{
		var result = await _useCase.Register(request);

		return result.Success ? Ok() : BadRequest(result);
	}

	[HttpPatch("{id}/cnh")]
	[Authorize(Roles = "entregador")]
	public IActionResult Patch([FromRoute] string id, [FromBody] PatchDriverLicenseImageRequest request)
	{
		var result = _useCase.Update(id, request);

		return result.Success ? Ok() : BadRequest(result);
	}

	[HttpGet]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Get()
	{
		return Ok(await _useCase.Get());
	}
}