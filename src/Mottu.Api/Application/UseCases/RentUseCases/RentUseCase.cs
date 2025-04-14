using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Application.Models;
using FluentValidation;

namespace Mottu.Api.Application.UseCases.RentUseCases;

public class RentUseCase : IRentUseCase
{
    private readonly IRepository<Rent> _rentRepository;
    private readonly IRepository<DeliveryMan> _deliveryManRepository;
    private readonly IRepository<Motorcycle> _motorcycleRepository;
    private readonly INotificationService _notificationService;
    private readonly IValidator<PostRentRequest> _validator;
    private readonly string _notificationKey = nameof(PostRentRequest);

    public RentUseCase(
        IRepository<Rent> rentRepository, 
        IRepository<DeliveryMan> deliveryManRepository, 
        IRepository<Motorcycle> motorcycleRepository, 
        INotificationService notificationService,
        IValidator<PostRentRequest> validator)
    {
        _rentRepository = rentRepository;
        _deliveryManRepository = deliveryManRepository;
        _motorcycleRepository = motorcycleRepository;
        _notificationService = notificationService;
        _validator = validator;
    }

    public void Create(PostRentRequest request)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notificationService.Add(new Notification(_notificationKey, error.ErrorMessage));
            }

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
            request.Plan, dailyValue);

        _rentRepository.Create(rent);
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