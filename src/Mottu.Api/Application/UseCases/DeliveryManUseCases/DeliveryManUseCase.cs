using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Application.Models;
using Mottu.Api.Infrastructure.Services.Storage;
using FluentValidation;
using Mottu.Api.Extensions;
using Mottu.Api.Application.Validators;

namespace Mottu.Api.Application.UseCases.DeliveryManUseCases;

public class DeliveryManUseCase(
    IRepository<DeliveryMan> repository,
    IStorageService storageService,
    IValidator<PostDeliveryManRequest> postDeliveryManRequestValidator,
    IValidator<PatchDriverLicenseImageRequest> patchDriverLicenseImageRequestValidator) : IDeliveryManUseCase
{
    private readonly IRepository<DeliveryMan> _repository = repository;
    private readonly IStorageService _storageService = storageService;
    private readonly IValidator<PostDeliveryManRequest> _postDeliveryManRequestValidator = postDeliveryManRequestValidator;
    private readonly IValidator<PatchDriverLicenseImageRequest> _patchDriverLicenseImageRequestValidator = patchDriverLicenseImageRequestValidator;

    public Result<string> Create(PostDeliveryManRequest request)
    {
        var validationResult = _postDeliveryManRequestValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Result<string>.Fail(validationResult.GetErrorMessages());
        }

        var driverLicenseImagePath = _storageService.SaveBase64Image(request.DriverLicenseImage);

        var deliveryMan = new DeliveryMan(request.Name, request.CompanyRegistrationNumber, request.DateOfBirth,
            new DriverLicense(request.DriverLicense, request.DriverLicenseType, driverLicenseImagePath));

        _repository.Create(deliveryMan);

        return Result<string>.Ok(string.Empty);
    }

    public Result<string> Update(string id, PatchDriverLicenseImageRequest request)
    {
        var validationResult = _patchDriverLicenseImageRequestValidator.Validate(request);

        if(!validationResult.IsValid)
        {
            return Result<string>.Fail(validationResult.GetErrorMessages());
        }

        var deliveryMan = _repository.GetFirst(x => x.Id == id);

        //todo: add unit test
        if(deliveryMan == null)
        {
            return Result<string>.Fail($"Entregador de id {id} não encontrado");
        }

        _storageService.DeleteImage(deliveryMan.DriverLicense.Image);

        var driverLicenseImagePath = _storageService.SaveBase64Image(request.DriverLicenseImage);

        deliveryMan.DriverLicense.UpdateImage(driverLicenseImagePath);

        _repository.Update(deliveryMan);

        return Result<string>.Ok(string.Empty);
    }

    public IEnumerable<GetDeliveryManResponse> Get()
    {
        var deliveryMen = _repository.GetCollection();

        return deliveryMen.Select(x => new GetDeliveryManResponse(x.Id, x.Name, x.CompanyRegistrationNumber, x.DateOfBirth,
            new GetDriverLicenseResponse(x.DriverLicense.Number, x.DriverLicense.Type, x.DriverLicense.Image)));
    }
}