using Microsoft.AspNetCore.Mvc;

using Mottu.Api.Infrastructure.Notifications;
using Mottu.Api.Models;
using Mottu.Api.UseCases.DeliveryManUseCases;

namespace Mottu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DeliveryMenController : ApiControllerBase
{
    private readonly IDeliveryManUseCase _useCase;
    private readonly ILogger<DeliveryMenController> _logger;

    public DeliveryMenController(
        IDeliveryManUseCase useCase,
        ILogger<DeliveryMenController> logger, 
        INotificationService notificationService) : base(notificationService)
    {
        _useCase = useCase;
        _logger = logger;
    }

    public IActionResult Register([FromBody] PostDeliveryManRequest request)
    {
        return Ok();
    }
}