using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.DeliveryManUseCases;

public interface IDeliveryManUseCase
{
    Result<string> Create(PostDeliveryManRequest request);
    Result<string> Update(string id, PatchDriverLicenseImageRequest request);
    IEnumerable<GetDeliveryManResponse> Get();
}