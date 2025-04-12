using Mottu.Api.Models;

namespace Mottu.Api.UseCases.DeliveryManUseCases;

public interface IDeliveryManUseCase
{
    void Create(PostDeliveryManRequest request);
    void Update(int id, PatchDriverLicenseImageRequest request);
}