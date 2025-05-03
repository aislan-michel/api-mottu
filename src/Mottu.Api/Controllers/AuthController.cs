using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Controllers;
using Mottu.Api.Infrastructure.Interfaces;

[ApiController]
[Route("api/autenticar")]
[Produces("application/json")]
[AllowAnonymous]
public class AuthController(ITokenService tokenService) : ApiControllerBase
{
    private readonly ITokenService _tokenService = tokenService;

    [HttpGet]
    public IActionResult Get([FromQuery] string role)
    {
        var token = _tokenService.GenerateToken(role);

        return Ok(token);
    }

    [HttpPost]
    public IActionResult Register([FromBody] RegisterUserRequest request)
    {
        return Ok();
    }
}