using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Infrastructure.Models;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
[Produces("application/json")]
//[Authorize(Roles = "admin")]
[AllowAnonymous]
public class UsersController(IRepository<User> repository) : ApiControllerBase
{
    private readonly IRepository<User> _repository = repository;

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_repository.GetCollection());
    }
}