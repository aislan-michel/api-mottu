using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Controllers;
using Mottu.Api.Application.Interfaces;

[ApiController]
[Route("api/autenticar")]
[Produces("application/json")]
[AllowAnonymous]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var result = await _authService.Login(request);

        if(!result.Success)
        {
            return BadRequest(result.GetMessages());
        }

        return Ok(new { token = result.Data });
    }
}