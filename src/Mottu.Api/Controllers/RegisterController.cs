using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Application.Interfaces;
using Mottu.Api.Controllers;

[ApiController]
[Route("api/cadastro")]
[Produces("application/json")]
[AllowAnonymous]
public class RegisterController(IAuthService authService, IDeliveryManUseCase deliveryManUseCase) : ApiControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IDeliveryManUseCase _deliveryManUseCase = deliveryManUseCase;

    [HttpPost("admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserRequest request)
    {
        request.Role = "admin";

        var result = await _authService.Register(request);

        if(!result.Success)
        {
            return BadRequest(result.GetMessages());
        }

        return Created();
    }

    [HttpPost("entregador")]
    public async Task<IActionResult> RegisterDeliveryMan([FromBody] RegisterDeliveryManRequest request)
    {
        var result = await _deliveryManUseCase.Register(request);

        if(!result.Success)
        {
            return BadRequest(result.GetMessages());
        }

        return Created();
    }
}