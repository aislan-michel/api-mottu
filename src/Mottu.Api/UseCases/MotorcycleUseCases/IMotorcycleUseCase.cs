
using Mottu.Api.Models;

namespace Mottu.Api.UseCases.MotorcycleUseCases;

public interface IMotorcycleUseCase
{
    void Create(PostMotorcycleRequest request);
}