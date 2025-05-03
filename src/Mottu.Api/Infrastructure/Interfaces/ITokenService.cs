namespace Mottu.Api.Infrastructure.Interfaces;

public interface ITokenService
{
    string GenerateToken(string role);
}