using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.RentUseCases;

public interface IRentUseCase
{
    Result<CreateRentResponse> Create(PostRentRequest request);
    Result<GetRentResponse?> GetById(string id);
    Result<UpdateRentResponse> Update(string id, PatchRentRequest request);
    IEnumerable<GetRentResponse> Get();
}