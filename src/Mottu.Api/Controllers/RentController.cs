using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Application.Interfaces;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/locacao")]
[Produces("application/json")]
[Authorize(Roles = "admin,entregador")]
public class RentController(
	IRentUseCase useCase,
	ILogger<RentController> logger) : ControllerBase
{
	private readonly IRentUseCase _useCase = useCase;
	private readonly ILogger<RentController> _logger = logger;

	[HttpPost]
	public IActionResult Post(PostRentRequest request)
	{
		var result = _useCase.Create(request);

		if (!result.Success)
		{
			return BadRequest(result);
		}

		return Ok();
	}

	[HttpGet("{id}")]
	public IActionResult GetById([FromRoute] string id)
	{
		var result = _useCase.GetById(id);

		if (!result.Success)
		{
			return NotFound();
		}

		return Ok(result.Data);
	}

	[HttpPatch("{id}/devolucao")]
	public IActionResult Update([FromRoute] string id, [FromBody] PatchRentRequest request)
	{
		var result = _useCase.Update(id, request);

		if (!result.Success)
		{
			return BadRequest(result);
		}

		return Ok();
	}

	[HttpGet()]
	public async Task<IActionResult> Get()
	{
		return Ok(await _useCase.Get());
	}
}