using Mottu.Api.Application.Interfaces;
using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases;

public class AdminUseCase(IAuthService authService) : IAdminUseCase
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<RegisterAdminResponse>> Register(RegisterAdminRequest request)
    {
        var result = await _authService.Register(new RegisterUserRequest()
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            Role = "admin"
        });

        if(!result.Success)
        {
            return Result<RegisterAdminResponse>.Fail(result.GetMessages());
        }

        return Result<RegisterAdminResponse>.Ok(new RegisterAdminResponse(result.Data!.UserId));
    }
}
