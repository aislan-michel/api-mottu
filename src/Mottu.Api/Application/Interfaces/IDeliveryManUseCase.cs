using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.Interfaces;

public interface IDeliveryManUseCase
{
    Task<Result<string>> Update(string id, PatchDriverLicenseImageRequest request);
    Task<IEnumerable<GetDeliveryManResponse>> Get();
    Task<Result<string>> Register(RegisterDeliveryManRequest request);
}