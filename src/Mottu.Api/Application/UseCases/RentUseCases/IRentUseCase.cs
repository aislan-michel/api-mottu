using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.RentUseCases;

public interface IRentUseCase
{
    void Create(PostRentRequest request);
    GetRentResponse? Get(int id);
    void Update(int id, PatchRentRequest request);
    IEnumerable<GetRentResponse> Get();
}