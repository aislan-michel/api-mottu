using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.MotorcycleUseCases;

public class MotorcycleUseCase : IMotorcycleUseCase
{
    private readonly INotificationService _notificationService;
    private readonly IRepository<Motorcycle> _motorcycleRepository;
    private readonly IRepository<Rent> _rentRepository;

    public MotorcycleUseCase(
        INotificationService notificationService,
        IRepository<Motorcycle> motorcycleRepository,
        IRepository<Rent> rentRepository)
    {
        _notificationService = notificationService;
        _motorcycleRepository = motorcycleRepository;
        _rentRepository = rentRepository;
    }

    public void Create(PostMotorcycleRequest request)
    {
        ValidatePostMotorcycleRequest(request);

        if (_notificationService.HaveNotifications())
        {
            return;
        }

        var motorcycle = new Motorcycle(request.Year, request.Model, request.Plate);

        _motorcycleRepository.Create(motorcycle);

        //todo: produces event
    }

    private void ValidatePostMotorcycleRequest(PostMotorcycleRequest request)
    {
        const string key = nameof(PostMotorcycleRequest);

        if (request.Year <= 0)
        {
            _notificationService.Add(new Notification(key, "Ano da moto menor ou igual a zero"));
        }

        if (string.IsNullOrWhiteSpace(request.Model))
        {
            _notificationService.Add(new Notification(key, "Modelo não pode ser nulo ou vazio"));
        }

        if (string.IsNullOrWhiteSpace(request.Plate))
        {
            _notificationService.Add(new Notification(key, "Placa não pode ser nulo ou vazio"));
        }

        //todo: i can register the same plate, the first in lowercase and the second in uppercase
        if (_motorcycleRepository.Exists(x => x.Plate.Equals(request.Plate, StringComparison.OrdinalIgnoreCase)))
        {
            _notificationService.Add(new Notification(key, $"Moto com a placa {request.Plate} já cadastrada"));
        }
    }

    public IEnumerable<GetMotorcycleResponse> Get(string? plate)
    {
        return _motorcycleRepository
            .GetCollection(string.IsNullOrWhiteSpace(plate) ? null : x => x.Plate == plate)
            .Select(x => new GetMotorcycleResponse(x.Id, x.Year, x.Model, x.Plate));
    }

    //todo: create unit tests
    public GetMotorcycleResponse? GetById(string id)
    {
        var motorcycle = _motorcycleRepository.GetFirst(x => x.Id == id);

        if(motorcycle == null)
        {
            return default;
        }

        return new GetMotorcycleResponse(motorcycle.Id, motorcycle.Year, motorcycle.Model, motorcycle.Plate);
    }

    public void Update(string id, PatchMotorcycleRequest request)
    {
        ValidatePatchMotorcycleRequest(id, request);

        if(_notificationService.HaveNotifications())
        {
            return;
        }

        var motorcycle = _motorcycleRepository.GetFirst(x => x.Id == id);

        if (motorcycle == null)
        {
            _notificationService.Add(new Notification(nameof(PatchMotorcycleRequest), "Moto não encontrada"));
            return;
        }

        motorcycle.UpdatePlate(request.Plate);

        _motorcycleRepository.Update(motorcycle);
    }

    private void ValidatePatchMotorcycleRequest(string id, PatchMotorcycleRequest request)
    {
        const string key = nameof(PatchMotorcycleRequest);

        if(string.IsNullOrWhiteSpace(request.Plate))
        {
            _notificationService.Add(new Notification(key, "Placa não pode ser nulo ou vazio"));
        }

        //todo: i can update the same plate, the first in lowercase and the second in uppercase
        if (_motorcycleRepository.Exists(x => x.Plate == request.Plate))
        {
            _notificationService.Add(new Notification(key, $"Moto com a placa {request.Plate} já cadastrada"));
        }
    }

    public void Delete(string id)
    {
        var motorcycle = _motorcycleRepository.GetFirst(x => x.Id == id);

        if(motorcycle == null)
        {
            _notificationService.Add(new Notification("", "Moto não encontrada"));
            return;
        }

        var rent = _rentRepository.GetFirst(x => x.Motorcycle.Id == id);

        if(rent != null)
        {
            _notificationService.Add(new Notification("", $"Moto possui registro de locação, id da locação: {rent.Id}"));
            return;
        }

        _motorcycleRepository.Delete(motorcycle);
    }
}