using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Notifications;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Models;

namespace Mottu.Api.UseCases.MotorcycleUseCases;

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
        Validate(request);

        if(_notificationService.HaveNotifications())
        {
            return;
        }

        var id = new Random().Next();

        var motorcycle = new Motorcycle(id, request.Year, request.Model, request.Plate);

        _repository.Save(motorcycle);
    }

    
	private void Validate(PostMotorcycleRequest request)
	{
		if(request.Year <= 0)
		{
			_notificationService.Add(BuildRequestNotification("Ano da moto menor ou igual a zero"));
		}

		if(string.IsNullOrWhiteSpace(request.Model))
		{
			_notificationService.Add(BuildRequestNotification("Modelo não pode ser nulo ou vazio"));
		}

		if(string.IsNullOrWhiteSpace(request.Plate))
		{
			_notificationService.Add(BuildRequestNotification("Placa não pode ser nulo ou vazio"));
		}
	}

    private Notification BuildRequestNotification(string message)
    {
        return new Notification(nameof(PostMotorcycleRequest), message);
    }
}