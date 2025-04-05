using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Notifications;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Models;

namespace Mottu.Api.UseCases.DeliveryManUseCases;

public class DeliveryManUseCase : IDeliveryManUseCase
{
    private readonly IRepository<DeliveryMan> _repository;
    private readonly INotificationService _notificationService;

    public DeliveryManUseCase(
        IRepository<DeliveryMan> repository, 
        INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public void Create(PostDeliveryManRequest request)
    {
        ValidatePostDeliveryManRequest(request);

        if(_notificationService.HaveNotifications())
        {
            return;
        }

        //todo: fix id to string (mandatory) and get from the request (optional)
        var id = new Random().Next();

        var deliveryMan = new DeliveryMan(id, request.Name, request.Cnpj, request.DateOfBirth,
            request.Cnh, request.CnhType, request.CnhImage);

        _repository.Create(deliveryMan);
    }

    private void ValidatePostDeliveryManRequest(PostDeliveryManRequest request)
    {
        const string key = nameof(PostDeliveryManRequest);

        var validCnhTypes = new string[3] {"A", "B", "A+B"};

        if(!validCnhTypes.Contains(request.CnhType))
        {
            _notificationService.Add(new Notification(key, $"Tipo da CNH inválida, tipos válidos são: {string.Join(", ", validCnhTypes)}"));
        }

        if(_repository.Exists(x => x.Cnpj == request.Cnpj))
        {
            _notificationService.Add(new Notification(key, $"Entregador com a CNPJ {request.Cnpj} já cadastrado"));
        }

        if(_repository.Exists(x => x.Cnh == request.Cnh))
        {
            _notificationService.Add(new Notification(key, $"Entregador com a CNH {request.Cnh} já cadastrado"));
        }
    }
}