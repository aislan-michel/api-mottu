
using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.MotorcycleUseCases;

public interface IMotorcycleUseCase
{
    void Create(PostMotorcycleRequest request);
    IEnumerable<GetMotorcycleResponse> Get(string? plate);
    GetMotorcycleResponse? Get(int id);
    void Update(int id, PatchMotorcycleRequest request);
    void Delete(int id);
}