
using Mottu.Api.Models;

namespace Mottu.Api.UseCases.MotorcycleUseCases;

public interface IMotorcycleUseCase
{
    void Create(PostMotorcycleRequest request);
    IEnumerable<GetMotorcycleResponse> Get(string? plate);
    GetMotorcycleResponse? Get(int id);
    void Update(int id, PatchMotorcycleRequest request);
    void Delete(int id);
}