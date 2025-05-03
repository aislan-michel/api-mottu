using Mottu.Api.Application.Models;

namespace Mottu.Api.Infrastructure.Interfaces;

public interface IAuthService
{
    Result<string> Register(RegisterUserRequest request);
    Result<string> Login(LoginUserRequest request);
}