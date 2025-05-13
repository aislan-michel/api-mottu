using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Application.Interfaces;

[ApiController]
[Route("api/cadastro")]
[Produces("application/json")]
[AllowAnonymous]
public class RegisterController(
    IAdminUseCase adminUseCase, 
    IDeliveryManUseCase deliveryManUseCase) : ControllerBase
{
    private readonly IAdminUseCase _adminUseCase = adminUseCase;
    private readonly IDeliveryManUseCase _deliveryManUseCase = deliveryManUseCase;

    [HttpPost("admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminRequest request)
    {
        var result = await _adminUseCase.Register(request);

        return result.Success ? Created() : BadRequest(result);
    }

    [HttpPost("entregador")]
    public async Task<IActionResult> RegisterDeliveryMan([FromBody] RegisterDeliveryManRequest request)
    {
        var result = await _deliveryManUseCase.Register(request);

        return result.Success ? Created() : BadRequest(result);
    }
}