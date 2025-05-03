using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.Interfaces;

public interface IDeliveryManUseCase
{
    Result<string> Create(PostDeliveryManRequest request);
    Result<string> Update(string id, PatchDriverLicenseImageRequest request);
    IEnumerable<GetDeliveryManResponse> Get();
}