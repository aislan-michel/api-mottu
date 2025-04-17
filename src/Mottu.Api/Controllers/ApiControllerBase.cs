using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Infrastructure.Services.Notifications;

namespace Mottu.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected readonly INotificationService _notificationService;

    protected ApiControllerBase(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    protected IActionResult InternalServerError()
    {
        return StatusCode(500, new ProblemDetails
        {
            Title = "Erro Interno no Servidor",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "Houve um erro inesperado ao processar sua solicitação.",
            Instance = HttpContext.Request.Path
        });
    }

    protected IActionResult UnprocessableEntity(string title, string detail)
    {
        return UnprocessableEntity(new ProblemDetails
        {
            Title = title,
            Status = StatusCodes.Status422UnprocessableEntity,
            Detail = detail,
            Instance = HttpContext.Request.Path
        });
    }

    protected IActionResult BadRequest(string detail)
    {
        return BadRequest(new ProblemDetails
        {
            Title = "bad request",
            Status = StatusCodes.Status400BadRequest,
            Detail = detail,
            Instance = HttpContext.Request.Path
        });
    }
}