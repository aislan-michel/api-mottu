using Mottu.Api.Models;

namespace Mottu.Api.UseCases.RentUseCases;

public interface IRentUseCase
{
    void Create(PostRentRequest request);
    GetRentResponse? Get(int id);
}