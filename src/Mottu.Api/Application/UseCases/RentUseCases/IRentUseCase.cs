using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.RentUseCases;

public interface IRentUseCase
{
    void Create(PostRentRequest request);
    GetRentResponse? GetById(string id);
    void Update(string id, PatchRentRequest request);
    IEnumerable<GetRentResponse> Get();
}