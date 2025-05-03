using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.Interfaces;

public interface IRentUseCase
{
    Result<string> Create(PostRentRequest request);
    Result<GetRentResponse?> GetById(string id);
    Result<string> Update(string id, PatchRentRequest request);
    IEnumerable<GetRentResponse> Get();
}