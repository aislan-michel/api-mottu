using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Application.Interfaces;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/locacao")]
[Produces("application/json")]
[Authorize(Roles = "admin,entregador")]
public class RentController(IRentUseCase useCase) : ControllerBase
{
	private readonly IRentUseCase _useCase = useCase;

	[HttpPost]
	public async Task<IActionResult> Post(PostRentRequest request)
	{
		var result = await _useCase.Create(request);

		return result.Success ? Ok() : BadRequest(result);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] string id)
	{
		var result = await _useCase.GetById(id);

		return result.Success ? Ok(result.Data) : NotFound();
	}

	[HttpPatch("{id}/devolucao")]
	public async Task<IActionResult> Update([FromRoute] string id, [FromBody] PatchRentRequest request)
	{
		var result = await _useCase.Update(id, request);

		return result.Success ? Ok() : BadRequest(result);
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		return Ok(await _useCase.Get());
	}
}