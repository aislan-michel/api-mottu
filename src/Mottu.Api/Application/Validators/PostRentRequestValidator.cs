using FluentValidation;

using Mottu.Api.Application.Interfaces;
using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;

namespace Mottu.Api.Application.Validators;

public class PostRentRequestValidator : AbstractValidator<PostRentRequest>
{
    private readonly IRepository<Motorcycle> _motorcycleRepository;
    private readonly ILoggedUserService _loggedUserService;
    private readonly int[] _validPlans = [7, 15, 30, 45, 50];

    public PostRentRequestValidator(
        IRepository<Motorcycle> motorcycleRepository,
        ILoggedUserService loggedUserService)
    {
        _motorcycleRepository = motorcycleRepository;
        _loggedUserService = loggedUserService;

        var deliveryManId = _loggedUserService.DeliveryManId;

        RuleFor(x => x.MotorcycleId)
            .MustAsync((id, cancellationToken) => ExistsMotorcycle(id)).WithMessage(x => $"Moto com id {x.MotorcycleId} não encontrada");

        RuleFor(x => x.Plan)
            .Must(BeAValidPlan).WithMessage($"Plano inválido, planos validos são: {string.Join(", ", _validPlans)}");
    }

    private async Task<bool> ExistsMotorcycle(string id) =>
        await _motorcycleRepository.Exists(x => x.Id == id);

    private bool BeAValidPlan(int plan) => 
        _validPlans.Contains(plan);
}
