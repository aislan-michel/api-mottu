using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;
using Mottu.Api.Controllers;
using Mottu.Api.Infrastructure.Interfaces;

[ApiController]
[Route("api/autenticar")]
[Produces("application/json")]
[AllowAnonymous]
public class AuthController(IAuthService authService) : ApiControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginUserRequest request)
    {
        var result = _authService.Login(request);

        if(!result.Success)
        {
            return BadRequest(result.GetMessages());
        }

        return Ok(new 
        {
            token = result.Data
        });
    }

    [HttpPost("registrar")]
    public IActionResult Register([FromBody] RegisterUserRequest request)
    {
        var result = _authService.Register(request);

        return Created();
    }
}