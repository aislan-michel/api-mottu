using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.Interfaces;

public interface IAdminUseCase
{
    Task<Result<RegisterAdminResponse>> Register(RegisterAdminRequest request);
}