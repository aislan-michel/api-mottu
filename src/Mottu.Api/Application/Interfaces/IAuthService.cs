using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.Interfaces;

//todo: separe register and login in different service files
public interface IAuthService
{
    Task<Result<RegisterUserResponse>> Register(RegisterUserRequest request);
    Task<Result<string>> Login(LoginUserRequest request);
}