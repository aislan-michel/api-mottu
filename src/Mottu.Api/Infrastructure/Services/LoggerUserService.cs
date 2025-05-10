using System.Security.Claims;

using Mottu.Api.Application.Interfaces;

namespace Mottu.Api.Infrastructure.Services;

public class LoggedUserService(IHttpContextAccessor httpContextAccessor) : ILoggedUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? DeliveryManId =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(CustomClaimTypes.DeliveryManId);
}