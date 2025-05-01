using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Interfaces;
using Mottu.Api.Controllers;

[ApiController]
[Route("api/autenticar")]
[Produces("application/json")]
[AllowAnonymous]
public class AuthController(IAuthService authService) : ApiControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpGet]
    public IActionResult Get([FromQuery] string role)
    {
        var token = _authService.GenerateToken(role);

        return Ok(token);
    }
}