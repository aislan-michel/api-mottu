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
    private readonly ILogger<RentUseCase> _logger;
    private readonly string _notificationKey = nameof(PostRentRequest);

    public RentUseCase(
        IRepository<Rent> rentRepository, 
        IRepository<DeliveryMan> deliveryManRepository, 
        IRepository<Motorcycle> motorcycleRepository, 
        INotificationService notificationService,
        IValidator<PostRentRequest> validator,
        ILogger<RentUseCase> logger)
    {
        _rentRepository = rentRepository;
        _deliveryManRepository = deliveryManRepository;
        _motorcycleRepository = motorcycleRepository;
        _notificationService = notificationService;
        _validator = validator;
        _logger = logger;
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
        var today = DateTime.Today;
        var startDate = new DateTime(today.Year, today.Month, today.Day, 00, 00, 00);
        var endDate = startDate.AddDays(request.Plan).AddSeconds(-1);
        var expectedEndDate = endDate;
        
        _logger.LogInformation("create a rent... start date: {startDate}, end date: {endDate} and expected end date: {expectedEndDate}", startDate, endDate, expectedEndDate);

        var rent = new Rent(id,
            deliveryMan, motorcycle,
            startDate, endDate, expectedEndDate,
            new Plan(request.Plan));

        _rentRepository.Create(rent);
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

        return new GetRentResponse(rent.Id, rent.Plan.DailyRate,
            rent.DeliveryMan.Id, rent.Motorcycle.Id, 
            rent.StartDate, rent.EndDate, rent.ExpectedEndDate,
            rent.ReturnDate, rent.TotalAmountPayable);
    }

    public void Update(int id, PatchRentRequest request)
    {
        if(request.ReturnDate == null || request.ReturnDate == DateTime.MinValue || request.ReturnDate == DateTime.MaxValue)
        {
            _notificationService.Add(new Notification("", "Data de devolução inválida"));
            return;
        }

        var rent = _rentRepository.GetFirst(x => x.Id == id);

        if(rent == null)
        {
            _notificationService.Add(new Notification("", $"Locação com id {id} não encontrada"));
            return;
        }

        rent.UpdateReturnDate(request.ReturnDate);
        rent.SetTotalAmountPayable();

        _rentRepository.Update(rent);
    }

    public IEnumerable<GetRentResponse> Get()
    {
        return _rentRepository.GetCollection().Select(rent => new GetRentResponse(
            rent.Id, rent.Plan.DailyRate,
            rent.DeliveryMan.Id, rent.Motorcycle.Id, 
            rent.StartDate, rent.EndDate, rent.ExpectedEndDate,
            rent.ReturnDate, rent.TotalAmountPayable));
    }
}