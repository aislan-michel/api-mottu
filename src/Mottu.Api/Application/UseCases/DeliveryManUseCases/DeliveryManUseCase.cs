using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Application.Models;
using Mottu.Api.Infrastructure.Services.Storage;
using FluentValidation;

namespace Mottu.Api.Application.UseCases.DeliveryManUseCases;

public class DeliveryManUseCase : IDeliveryManUseCase
{
    private readonly IRepository<DeliveryMan> _repository;
    private readonly INotificationService _notificationService;
    private readonly IStorageService _storageService;
    private readonly IValidator<PostDeliveryManRequest> _validator;

    public DeliveryManUseCase(
        IRepository<DeliveryMan> repository,
        INotificationService notificationService,
        IStorageService storageService,
        IValidator<PostDeliveryManRequest> validator)
    {
        _repository = repository;
        _notificationService = notificationService;
        _storageService = storageService;
        _validator = validator;
    }

    public void Create(PostDeliveryManRequest request)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notificationService.Add(new Notification("", error.ErrorMessage));
            }

            return;
        }

        var driverLicenseImagePath = _storageService.SaveBase64Image(request.DriverLicenseImage);

        var deliveryMan = new DeliveryMan(request.Name, request.CompanyRegistrationNumber, request.DateOfBirth,
            new DriverLicense(request.DriverLicense, request.DriverLicenseType, driverLicenseImagePath));

        _repository.Create(deliveryMan);
    }

    public void Update(string id, PatchDriverLicenseImageRequest request)
    {
        ValidatePatchDriverLicenseImageRequest(id, request);

        if(_notificationService.HaveNotifications())
        {
            return;
        }

        var deliveryMan = _repository.GetFirst(x => x.Id == id);

        //todo: add unit test
        if(deliveryMan == null)
        {
            _notificationService.Add(new Notification("", $"Entregador de id {id} não encontrado"));
            return;
        }

        _storageService.DeleteImage(deliveryMan.DriverLicense.Image);

        var driverLicenseImagePath = _storageService.SaveBase64Image(request.DriverLicenseImage);

        deliveryMan.DriverLicense.UpdateImage(driverLicenseImagePath);

        _repository.Update(deliveryMan);
    }

    private void ValidatePatchDriverLicenseImageRequest(string id, PatchDriverLicenseImageRequest request)
    {
        const string key = nameof(PatchDriverLicenseImageRequest);

        if(!_repository.Exists(x => x.Id == id))
        {
            _notificationService.Add(new Notification(key, $"Entregador de id {id} não encontrado"));
        }

        if(string.IsNullOrWhiteSpace(request.DriverLicenseImage))
        {
            _notificationService.Add(new Notification(key, $"Imagem não pode ser nula ou vazia"));
        }
    }

    public IEnumerable<GetDeliveryManResponse> Get()
    {
        var deliveryMen = _repository.GetCollection();

        return deliveryMen.Select(x => new GetDeliveryManResponse(x.Id, x.Name, x.CompanyRegistrationNumber, x.DateOfBirth,
            new GetDriverLicenseResponse(x.DriverLicense.Number, x.DriverLicense.Type, x.DriverLicense.Image)));
    }
}