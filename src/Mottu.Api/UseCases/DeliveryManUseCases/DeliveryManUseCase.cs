using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Models;
using Mottu.Api.Infrastructure.Services.Storage;

namespace Mottu.Api.UseCases.DeliveryManUseCases;

public class DeliveryManUseCase : IDeliveryManUseCase
{
    private readonly IRepository<DeliveryMan> _repository;
    private readonly INotificationService _notificationService;
    private readonly IStorageService _storageService;

    public DeliveryManUseCase(
        IRepository<DeliveryMan> repository,
        INotificationService notificationService,
        IStorageService storageService)
    {
        _repository = repository;
        _notificationService = notificationService;
        _storageService = storageService;
    }

    public void Create(PostDeliveryManRequest request)
    {
        ValidatePostDeliveryManRequest(request);

        if (_notificationService.HaveNotifications())
        {
            return;
        }

        var driverLicenseImagePath = _storageService.SaveBase64Image(request.DriverLicenseImage);

        //todo: fix id to string (mandatory) and get from the request (optional)
        var id = new Random().Next();

        var deliveryMan = new DeliveryMan(id, request.Name, request.CompanyRegistrationNumber, request.DateOfBirth,
            new DriverLicense(request.DriverLicense, request.DriverLicenseType, driverLicenseImagePath));

        _repository.Create(deliveryMan);
    }

    private void ValidatePostDeliveryManRequest(PostDeliveryManRequest request)
    {
        const string key = nameof(PostDeliveryManRequest);

        var validDriverLicenseTypes = new string[3] { "A", "B", "A+B" };

        if (!validDriverLicenseTypes.Contains(request.DriverLicenseType))
        {
            _notificationService.Add(new Notification(key, $"Tipo da CNH inválida, tipos válidos são: {string.Join(", ", validDriverLicenseTypes)}"));
        }

        if (_repository.Exists(x => x.CompanyRegistrationNumber == request.CompanyRegistrationNumber))
        {
            _notificationService.Add(new Notification(key, $"Entregador com a CNPJ {request.CompanyRegistrationNumber} já cadastrado"));
        }

        if (_repository.Exists(x => x.DriverLicense.Number == request.DriverLicense))
        {
            _notificationService.Add(new Notification(key, $"Entregador com a CNH {request.DriverLicense} já cadastrado"));
        }
    }
}