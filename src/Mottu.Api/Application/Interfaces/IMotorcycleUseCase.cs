
using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.Interfaces;

public interface IMotorcycleUseCase
{
    Task<Result<string>> Create(PostMotorcycleRequest request);
    Task<IEnumerable<GetMotorcycleResponse>> Get(string? plate);
    Task<GetMotorcycleResponse?> GetById(string id);
    Task<Result<string>> Update(string id, PatchMotorcycleRequest request);
    Task<Result<string>> Delete(string id);
}