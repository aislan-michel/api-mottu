using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.Interfaces;

public interface IRentUseCase
{
    Task<Result<string>> Create(PostRentRequest request);
    Task<Result<GetRentResponse?>> GetById(string id);
    Task<Result<string>> Update(string id, PatchRentRequest request);
    Task<IEnumerable<GetRentResponse>> Get();
}