using FluentValidation;

using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;

namespace Mottu.Api.Application.Validators;

public class PatchMotorcycleRequestValidator : AbstractValidator<PatchMotorcycleRequest>
{
    private readonly IRepository<Motorcycle> _motorcycleRepository;

    public PatchMotorcycleRequestValidator(IRepository<Motorcycle> motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;

        RuleFor(x => x.Plate)
            .NotNull().NotEmpty().WithMessage("Placa não pode ser nulo ou vazio")
            .Must(ExistsPlate).WithMessage(x => $"Moto com a placa {x.Plate} já cadastrada");
    }

    private bool ExistsPlate(string? plate) => 
        !_motorcycleRepository.Exists(x => x.Plate.Equals(plate, StringComparison.OrdinalIgnoreCase));
}