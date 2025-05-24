using FluentValidation;

using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;

namespace Mottu.Api.Application.Validators;

public class PatchMotorcycleRequestValidator : AbstractValidator<PatchMotorcycleRequest>
{
    private readonly IRepository<Motorcycle> _motorcycleRepository;

    public PatchMotorcycleRequestValidator(IRepository<Motorcycle> motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;

        RuleFor(x => x.Plate)
            .NotNull().NotEmpty().WithMessage("Placa não pode ser nulo ou vazio")
            .MustAsync((x, cancellationToken) => ExistsPlate(x)).WithMessage(x => $"Moto com a placa {x.Plate} já cadastrada");
    }

    private async Task<bool> ExistsPlate(string? plate) => 
        !await _motorcycleRepository.Exists(x => x.Plate == plate);//(x => x.Plate.Equals(plate, StringComparison.OrdinalIgnoreCase));
}