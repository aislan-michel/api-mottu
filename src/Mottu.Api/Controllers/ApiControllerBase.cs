using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Application.Models;

namespace Mottu.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult InternalServerError()
    {
        return StatusCode(500, Result<string>.Fail("Houve um erro inesperado ao processar sua solicitação - controller base."));
    }

    protected IActionResult BadRequest(string message)
    {
        return StatusCode(400, Result<string>.Fail(message));
    }
}