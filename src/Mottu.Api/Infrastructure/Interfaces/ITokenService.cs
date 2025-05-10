using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Infrastructure.Interfaces;

public interface ITokenService
{
    string GenerateToken(ApplicationUser user, string? deliveryManId, IList<string> roles);
}