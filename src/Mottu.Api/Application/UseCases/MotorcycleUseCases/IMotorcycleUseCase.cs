
using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.MotorcycleUseCases;

public interface IMotorcycleUseCase
{
    void Create(PostMotorcycleRequest request);
    IEnumerable<GetMotorcycleResponse> Get(string? plate);
    GetMotorcycleResponse? GetById(string id);
    void Update(string id, PatchMotorcycleRequest request);
    void Delete(string id);
}