using Mottu.Api.Application.Models;

namespace Mottu.Api.Infrastructure.Interfaces;

public interface IAuthService
{
    Task<Result<string>> Register(RegisterUserRequest request);
    Task<Result<string>> Login(LoginUserRequest request);
}