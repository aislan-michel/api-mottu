using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.UseCases.MotorcycleUseCases;

public class MotorcycleUseCase : IMotorcycleUseCase
{
    private readonly INotificationService _notificationService;
    private readonly IRepository<Motorcycle> _repository;

    public MotorcycleUseCase(
        INotificationService notificationService,
        IRepository<Motorcycle> repository)
    {
        _notificationService = notificationService;
        _repository = repository;
    }

    public void Create(PostMotorcycleRequest request)
    {
        ValidatePostMotorcycleRequest(request);

        if (_notificationService.HaveNotifications())
        {
            return;
        }

        //todo: fix id to string (mandatory) and get from the request (optional)
        var id = new Random().Next();

        var motorcycle = new Motorcycle(id, request.Year, request.Model, request.Plate);

        _repository.Create(motorcycle);

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
        if (_repository.Exists(x => x.Plate == request.Plate))
        {
            _notificationService.Add(new Notification(key, $"Moto com a placa {request.Plate} já cadastrada"));
        }
    }

    public IEnumerable<GetMotorcycleResponse> Get(string? plate)
    {
        return _repository
            .GetCollection(string.IsNullOrWhiteSpace(plate) ? null : x => x.Plate == plate)
            .Select(x => new GetMotorcycleResponse(x.Id, x.Year, x.Model, x.Plate));
    }

    //todo: create unit tests
    public GetMotorcycleResponse? Get(int id)
    {
        if(id <= 0)
        {
            return default;
        }

        var motorcycle = _repository.GetFirst(x => x.Id == id);

        if(motorcycle == null)
        {
            return default;
        }

        return new GetMotorcycleResponse(motorcycle.Id, motorcycle.Year, motorcycle.Model, motorcycle.Plate);
    }

    public void Update(int id, PatchMotorcycleRequest request)
    {
        ValidatePatchMotorcycleRequest(id, request);

        if(_notificationService.HaveNotifications())
        {
            return;
        }

        var motorcycle = _repository.GetFirst(x => x.Id == id);

        if (motorcycle == null)
        {
            _notificationService.Add(new Notification(nameof(PatchMotorcycleRequest), "Moto não encontrada"));
            return;
        }

        motorcycle.UpdatePlate(request.Plate);

        _repository.Update(motorcycle);
    }

    private void ValidatePatchMotorcycleRequest(int id, PatchMotorcycleRequest request)
    {
        const string key = nameof(PatchMotorcycleRequest);

        if(id <= 0)
        {
            _notificationService.Add(new Notification(key, "Id não pode ser menor ou igual a zero"));
        }

        if(string.IsNullOrWhiteSpace(request.Plate))
        {
            _notificationService.Add(new Notification(key, "Placa não pode ser nulo ou vazio"));
        }

        //todo: i can update the same plate, the first in lowercase and the second in uppercase
        if (_repository.Exists(x => x.Plate == request.Plate))
        {
            _notificationService.Add(new Notification(key, $"Moto com a placa {request.Plate} já cadastrada"));
        }
    }

    public void Delete(int id)
    {
        if(id <= 0)
        {
            _notificationService.Add(new Notification("", "Id não pode ser menor ou igual a zero"));
            return;
        }

        var motorcycle = _repository.GetFirst(x => x.Id == id);

        if(motorcycle == null)
        {
            _notificationService.Add(new Notification("", "Moto não encontrada"));
            return;
        }

        //todo: verify if dont exists rent
        _repository.Delete(motorcycle);
    }
}