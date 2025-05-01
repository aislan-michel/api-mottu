namespace Mottu.Api.Application.Interfaces;

public interface IAuthService
{
    string GenerateToken(string role);
}