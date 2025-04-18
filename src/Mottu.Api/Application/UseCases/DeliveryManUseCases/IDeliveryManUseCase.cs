using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.DeliveryManUseCases;

public interface IDeliveryManUseCase
{
    void Create(PostDeliveryManRequest request);
    void Update(string id, PatchDriverLicenseImageRequest request);
    IEnumerable<GetDeliveryManResponse> Get();
}