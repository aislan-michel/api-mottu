namespace Mottu.Api.Infrastructure.Interfaces;

public interface ITokenService
{
    string GenerateToken(IList<string> roles);
}