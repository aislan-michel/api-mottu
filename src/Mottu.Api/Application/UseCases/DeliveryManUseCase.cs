using Mottu.Api.Domain.Entities;
using Mottu.Api.Application.Models;
using FluentValidation;
using Mottu.Api.Extensions;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Application.Interfaces;

namespace Mottu.Api.Application.UseCases;

//todo: separar use cases by case c:
public class DeliveryManUseCase(
    IUnitOfWork unitOfWork,
    IStorageService storageService,
    IValidator<RegisterDeliveryManRequest> registerDeliveryManRequestValidator,
    IValidator<PatchDriverLicenseImageRequest> patchDriverLicenseImageRequestValidator,
    IAuthService authService) : IDeliveryManUseCase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStorageService _storageService = storageService;
    private readonly IValidator<RegisterDeliveryManRequest> _registerDeliveryManRequestValidator = registerDeliveryManRequestValidator;
    private readonly IValidator<PatchDriverLicenseImageRequest> _patchDriverLicenseImageRequestValidator = patchDriverLicenseImageRequestValidator;
    private readonly IAuthService _authService = authService;

    public async Task<Result<string>> Register(RegisterDeliveryManRequest request)
    {
        var validationResult = await _registerDeliveryManRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Result<string>.Fail(validationResult.GetErrorMessages());
        }

        await _unitOfWork.BeginTransaction();
        try
        {
            var registerUserResult = await _authService.Register(new RegisterUserRequest()
            {
                Email = request.Email,
                Password = request.Password,
                Role = "entregador",
                Username = request.Username
            });

            if (!registerUserResult.Success)
            {
                await _unitOfWork.RollbackTransaction();
                return Result<string>.Fail(registerUserResult.GetMessages());
            }

            await Create(new PostDeliveryManRequest()
            {
                Name = request.Name,
                CompanyRegistrationNumber = request.CompanyRegistrationNumber,
                DateOfBirth = request.DateOfBirth,
                DriverLicense = request.DriverLicense,
                DriverLicenseImageBase64 = request.DriverLicenseImageBase64,
                DriverLicenseType = request.DriverLicenseType
            }, registerUserResult.Data!.UserId);

            await _unitOfWork.CommitTransaction();
            return Result<string>.Ok("sucesso");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    private async Task Create(PostDeliveryManRequest request, string userId)
    {
        string? driverLicenseImagePath = null;
        if (!string.IsNullOrWhiteSpace(request.DriverLicenseImageBase64))
        {
            driverLicenseImagePath = _storageService.SaveBase64Image(request.DriverLicenseImageBase64);
        }

        var deliveryMan = new DeliveryMan(request.Name, request.CompanyRegistrationNumber, request.DateOfBirth,
            new DriverLicense(request.DriverLicense, request.DriverLicenseType, driverLicenseImagePath), userId);

        await _unitOfWork.DeliveryMen.Create(deliveryMan);
    }

    public async Task<Result<string>> Update(string id, PatchDriverLicenseImageRequest request)
    {
        var validationResult = _patchDriverLicenseImageRequestValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Result<string>.Fail(validationResult.GetErrorMessages());
        }

        var deliveryMan = await _unitOfWork.DeliveryMen.GetFirst(x => x.Id == id);

        if (deliveryMan == null)
        {
            return Result<string>.Fail($"Entregador de id {id} não encontrado");
        }

        if (!string.IsNullOrWhiteSpace(deliveryMan.DriverLicense.ImagePath))
        {
            _storageService.DeleteImage(deliveryMan.DriverLicense.ImagePath);
        }

        var driverLicenseImagePath = _storageService.SaveBase64Image(request.DriverLicenseImage);

        deliveryMan.DriverLicense.UpdateImagePath(driverLicenseImagePath);

        await _unitOfWork.DeliveryMen.Update(deliveryMan);

        return Result<string>.Ok(string.Empty);
    }

    public async Task<IEnumerable<GetDeliveryManResponse>> Get()
    {
        var deliveryMen = await _unitOfWork.DeliveryMen.GetCollection();

        return deliveryMen.Select(x => new GetDeliveryManResponse(x.Id, x.Name, x.CompanyRegistrationNumber, x.DateOfBirth,
            new GetDriverLicenseResponse(x.DriverLicense.Number, x.DriverLicense.Type, x.DriverLicense.ImagePath)));
    }
}