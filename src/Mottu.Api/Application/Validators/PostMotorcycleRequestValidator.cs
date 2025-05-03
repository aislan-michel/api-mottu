using FluentValidation;

using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;

namespace Mottu.Api.Application.Validators;

public class PostMotorcycleRequestValidator : AbstractValidator<PostMotorcycleRequest>
{
    private readonly IRepository<Motorcycle> _motorcycleRepository;

    public PostMotorcycleRequestValidator(IRepository<Motorcycle> motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;

        RuleFor(x => x.Year)
            .GreaterThan(0).WithMessage("Ano da moto não pode ser menor ou igual a zero");

        RuleFor(x => x.Model)
            .NotNull().NotEmpty().WithMessage("Modelo não pode ser nulo ou vazio");

        RuleFor(x => x.Plate)
            .NotNull().NotEmpty().WithMessage("Placa não pode ser nulo ou vazio")
            .Must(ExistsPlate).WithMessage(x => $"Moto com a placa {x.Plate} já cadastrada");
    }

    private bool ExistsPlate(string? plate) => 
        !_motorcycleRepository.Exists(x => x.Plate.Equals(plate, StringComparison.OrdinalIgnoreCase));
}