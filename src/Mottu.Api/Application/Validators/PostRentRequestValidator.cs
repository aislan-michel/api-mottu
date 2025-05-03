using FluentValidation;

using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;

namespace Mottu.Api.Application.Validators;

public class PostRentRequestValidator : AbstractValidator<PostRentRequest>
{
    private readonly IRepository<DeliveryMan> _deliveryManRepository;
    private readonly IRepository<Motorcycle> _motorcycleRepository;
    private readonly int[] _validPlans = [7, 15, 30, 45, 50];

    public PostRentRequestValidator(
        IRepository<DeliveryMan> deliveryManRepository,
        IRepository<Motorcycle> motorcycleRepository)
    {
        _deliveryManRepository = deliveryManRepository;
        _motorcycleRepository = motorcycleRepository;

        RuleFor(x => x.DeliveryManId)
            .Must(ExistsDeliveryMan).WithMessage(x => $"Entregador com id {x.DeliveryManId} não encontrado");

        RuleFor(x => x.MotorcycleId)
            .Must(ExistsMotorcycle).WithMessage(x => $"Moto com id {x.MotorcycleId} não encontrada");

        RuleFor(x => x.Plan)
            .Must(BeAValidPlan).WithMessage($"Plano inválido, planos validos são: {string.Join(", ", _validPlans)}");
    }

    private bool ExistsDeliveryMan(string id) =>
        _deliveryManRepository.Exists(x => x.Id == id);

    private bool ExistsMotorcycle(string id) =>
        _motorcycleRepository.Exists(x => x.Id == id);

    private bool BeAValidPlan(int plan) => 
        _validPlans.Contains(plan);
}
