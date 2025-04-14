using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Models;

namespace Mottu.Api.UseCases.RentUseCases;

public class RentUseCase : IRentUseCase
{
    private readonly IRepository<Rent> _rentRepository;
    private readonly IRepository<DeliveryMan> _deliveryManRepository;
    private readonly IRepository<Motorcycle> _motorcycleRepository;
    private readonly INotificationService _notificationService;
    private readonly string _notificationKey = nameof(PostRentRequest);

    public RentUseCase(
        IRepository<Rent> rentRepository, 
        IRepository<DeliveryMan> deliveryManRepository, 
        IRepository<Motorcycle> motorcycleRepository, 
        INotificationService notificationService)
    {
        _rentRepository = rentRepository;
        _deliveryManRepository = deliveryManRepository;
        _motorcycleRepository = motorcycleRepository;
        _notificationService = notificationService;
    }

    public void Create(PostRentRequest request)
    {
        ValidatePostRentRequest(request);

        if(_notificationService.HaveNotifications())
        {
            return;
        }

        var deliveryMan = _deliveryManRepository.GetFirst(x => x.Id == request.DeliveryManId);

        if(deliveryMan == null)
        {
            _notificationService.Add(new Notification(_notificationKey, "Entregador não encontrado"));
            return;
        }

        if(!deliveryMan.DriverLicense.TypeIsA())
        {
            _notificationService.Add(new Notification(_notificationKey, "Tipo da CNH do entregador é diferente de A"));
            return;
        }

        var motorcycle = _motorcycleRepository.GetFirst(x => x.Id == request.MotorcycleId);

        if(motorcycle == null)
        {
            _notificationService.Add(new Notification(_notificationKey, "Moto não encontrada"));
            return;
        }

        var id = new Random().Next();
        var dailyValue = CalculateDailyValue(request.Plan);

        var rent = new Rent(id,
            deliveryMan, motorcycle,
            request.StartDate, request.EndDate, request.ExpectedEndDate,
            request.Plan, dailyValue, null);

        _rentRepository.Create(rent);
    }

    private void ValidatePostRentRequest(PostRentRequest request)
    {
        if(request.DeliveryManId <= 0)
        {
            _notificationService.Add(new Notification(_notificationKey, "Id do entregador inválido"));
        }

        if(!_deliveryManRepository.Exists(x => x.Id == request.DeliveryManId))
        {
            _notificationService.Add(new Notification(_notificationKey, $"Entregador com id {request.DeliveryManId} não encontrado"));
        }

        if(request.MotorcycleId <= 0)
        {
            _notificationService.Add(new Notification(_notificationKey, "Id da moto inválido"));
        }

        if(!_motorcycleRepository.Exists(x => x.Id == request.MotorcycleId))
        {
            _notificationService.Add(new Notification(_notificationKey, $"Moto com id {request.MotorcycleId} não encontrado"));
        }

        if(request.StartDate == DateTime.MinValue || request.StartDate == DateTime.MaxValue)
        {
            _notificationService.Add(new Notification(_notificationKey, "Data de inicio inválida"));
        }

        if(request.EndDate == DateTime.MinValue || request.StartDate == DateTime.MaxValue)
        {
            _notificationService.Add(new Notification(_notificationKey, "Data de término inválida"));
        }

        if(request.ExpectedEndDate == DateTime.MinValue || request.ExpectedEndDate == DateTime.MaxValue)
        {
            _notificationService.Add(new Notification(_notificationKey, "Data de término prevista inválida"));
        }

        if(request.Plan <= 0)
        {
            _notificationService.Add(new Notification(_notificationKey, "Plano inválido"));
        }
    }

    private decimal CalculateDailyValue(int plan)
    {
        return plan switch
        {
            7 => 30,
            15 => 28,
            30 => 22,
            45 => 20,
            50 => 18,
            _ => decimal.Zero,
        };
    }

    public GetRentResponse? Get(int id)
    {
        if(id <= 0)
        {
            return default;
        }

        var rent = _rentRepository.GetFirst(x => x.Id == x.Id);

        if(rent == null)
        {
            return default;
        }

        return new GetRentResponse(rent.Id, rent.DailyValue,
            rent.DeliveryMan.Id, rent.Motorcycle.Id, 
            rent.StartDate, rent.EndDate, rent.ExpectedEndDate,
            rent.ReturnDate);
    }
}