using FluentValidation;

using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;

namespace Mottu.Api.Application.Validators;

public class PostRentRequestValidator : AbstractValidator<PostRentRequest>
{
    private readonly IRepository<DeliveryMan> _deliveryManRepository;
    private readonly IRepository<Motorcycle> _motorcycleRepository;

    public PostRentRequestValidator(
        IRepository<DeliveryMan> deliveryManRepository,
        IRepository<Motorcycle> motorcycleRepository)
    {
        _deliveryManRepository = deliveryManRepository;
        _motorcycleRepository = motorcycleRepository;

        RuleFor(x => x.DeliveryManId)
            .GreaterThan(0).WithMessage("Id do entregador inválido")
            .Must(ExistsDeliveryMan).WithMessage(x => $"Entregador com id {x.DeliveryManId} não encontrado");

        RuleFor(x => x.MotorcycleId)
            .GreaterThan(0).WithMessage("Id da moto inválido")
            .Must(ExistsMotorcycle).WithMessage(x => $"Moto com id {x.MotorcycleId} não encontrada");

        RuleFor(x => x.StartDate)
            .Must(BeAValidDate).WithMessage("Data de início inválida");

        RuleFor(x => x.EndDate)
            .Must(BeAValidDate).WithMessage("Data de término inválida");

        RuleFor(x => x.ExpectedEndDate)
            .Must(BeAValidDate).WithMessage("Data de término prevista inválida");

        RuleFor(x => x.Plan)
            .GreaterThan(0).WithMessage("Plano inválido");
    }

    private bool ExistsDeliveryMan(int id) =>
        _deliveryManRepository.Exists(x => x.Id == id);

    private bool ExistsMotorcycle(int id) =>
        _motorcycleRepository.Exists(x => x.Id == id);

    private bool BeAValidDate(DateTime date) =>
        date != DateTime.MinValue && date != DateTime.MaxValue;
}
