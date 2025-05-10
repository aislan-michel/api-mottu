
using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.Interfaces;

public interface IMotorcycleUseCase
{
    Result<string> Create(PostMotorcycleRequest request);
    IEnumerable<GetMotorcycleResponse> Get(string? plate);
    GetMotorcycleResponse? GetById(string id);
    Result<string> Update(string id, PatchMotorcycleRequest request);
    Result<string> Delete(string id);
}