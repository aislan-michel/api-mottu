using Mottu.Api.Domain.Entities;
using Mottu.Api.Application.Models;
using FluentValidation;
using Mottu.Api.Extensions;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Application.Interfaces;
using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Application.UseCases;

//todo: separar use cases by case c:
public class DeliveryManUseCase(
    IRepository<DeliveryMan> repository,
    IStorageService storageService,
    IValidator<PostDeliveryManRequest> postDeliveryManRequestValidator,
    IValidator<PatchDriverLicenseImageRequest> patchDriverLicenseImageRequestValidator,
    IAuthService authService,
    AppDbContext appDbContext) : IDeliveryManUseCase
{
    private readonly IRepository<DeliveryMan> _repository = repository;
    private readonly IStorageService _storageService = storageService;
    private readonly IValidator<PostDeliveryManRequest> _postDeliveryManRequestValidator = postDeliveryManRequestValidator;
    private readonly IValidator<PatchDriverLicenseImageRequest> _patchDriverLicenseImageRequestValidator = patchDriverLicenseImageRequestValidator;
    private readonly IAuthService _authService = authService;
    private readonly AppDbContext _appDbContext = appDbContext;

    public Result<string> Create(PostDeliveryManRequest request, string userId)
    {
        var validationResult = _postDeliveryManRequestValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Result<string>.Fail(validationResult.GetErrorMessages());
        }

        //todo: imagem da cnh não é obrigatório, mas, usuário ficara "inativado" até enviar
        //ou seja, não podera alugar uma moto
        string? driverLicenseImagePath = null;
        if(!string.IsNullOrWhiteSpace(request.DriverLicenseImageBase64))
        {
            driverLicenseImagePath = _storageService.SaveBase64Image(request.DriverLicenseImageBase64);
        }

        var deliveryMan = new DeliveryMan(request.Name, request.CompanyRegistrationNumber, request.DateOfBirth,
            new DriverLicense(request.DriverLicense, request.DriverLicenseType, driverLicenseImagePath), userId);

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

        if(deliveryMan == null)
        {
            return Result<string>.Fail($"Entregador de id {id} não encontrado");
        }

        _storageService.DeleteImage(deliveryMan.DriverLicense.ImagePath);

        var driverLicenseImagePath = _storageService.SaveBase64Image(request.DriverLicenseImage);

        deliveryMan.DriverLicense.UpdateImagePath(driverLicenseImagePath);

        _repository.Update(deliveryMan);

        return Result<string>.Ok(string.Empty);
    }

    public async Task<IEnumerable<GetDeliveryManResponse>> Get()
    {
        var deliveryMen = await _repository.GetCollection();

        return deliveryMen.Select(x => new GetDeliveryManResponse(x.Id, x.Name, x.CompanyRegistrationNumber, x.DateOfBirth,
            new GetDriverLicenseResponse(x.DriverLicense.Number, x.DriverLicense.Type, x.DriverLicense.ImagePath)));
    }

    public async Task<Result<string>> Register(RegisterDeliveryManRequest request)
    {
        //refactor, add UnitOfWork pattern
        var transaction = await _appDbContext.Database.BeginTransactionAsync();

        try
        {
            var registerUserResponse = await _authService.Register(new RegisterUserRequest()
            {
                Email = request.Email,
                Password = request.Password,
                Role = "entregador",
                Username = request.Username
            });

            if(!registerUserResponse.Success)
            {
                transaction.Rollback();
                return Result<string>.Fail(registerUserResponse.GetMessages());
            }

            var postDeliveryManResponse = Create(new PostDeliveryManRequest()
            {
                Name = request.Name,
                CompanyRegistrationNumber = request.CompanyRegistrationNumber,
                DateOfBirth = request.DateOfBirth,
                DriverLicense = request.DriverLicense,
                DriverLicenseImageBase64 = request.DriverLicenseImage,
                DriverLicenseType = request.DriverLicenseType
            }, registerUserResponse.Data.UserId);

            if(!postDeliveryManResponse.Success)
            {
                transaction.Rollback();
                return Result<string>.Fail(postDeliveryManResponse.GetMessages());
            }

            transaction.Commit();
            return Result<string>.Ok("sucesso");
        }
        catch(Exception)
        {   
            await transaction.RollbackAsync();
            throw;
        }
    }
}