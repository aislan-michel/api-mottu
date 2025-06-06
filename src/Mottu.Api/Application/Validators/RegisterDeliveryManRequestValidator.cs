using FluentValidation;

using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;

namespace Mottu.Api.Application.Validators;

public class RegisterDeliveryManRequestValidator : AbstractValidator<RegisterDeliveryManRequest>
{
    private readonly IRepository<DeliveryMan> _deliveryManRepository;
    private readonly string[] _validDriverLicenseTypes = ["A", "B", "A+B"];

    public RegisterDeliveryManRequestValidator(
        IRepository<DeliveryMan> deliveryManRepository)
    {
        _deliveryManRepository = deliveryManRepository;

        RuleFor(x => x.DriverLicenseType)
            .Must(BeAValidDriverLicenseType).WithMessage($"Tipo da CNH inválida, tipos válidos são: {string.Join(", ", _validDriverLicenseTypes)}");

        RuleFor(x => x.CompanyRegistrationNumber)
            .Must(IsDigitsOnly).WithMessage("CNPJ deve conter apenas números")
            .MustAsync((x, cancellationToken) => ExistsCompanyRegistrationNumber(x)).WithMessage(x => $"Entregador com a CNPJ {x.CompanyRegistrationNumber} já cadastrado");

        RuleFor(x => x.DriverLicense)
            .Must(IsDigitsOnly).WithMessage("CNH deve conter apenas números")
            .MustAsync((x, cancellationToken) => ExistsDriverLicense(x)).WithMessage(x => $"Entregador com a CNH {x.DriverLicense} já cadastrado");
    }

    private bool BeAValidDriverLicenseType(string? type) =>
        _validDriverLicenseTypes.Contains(type);

    private async Task<bool> ExistsCompanyRegistrationNumber(string? companyRegistrationNumber) =>
        !await _deliveryManRepository.Exists(x => x.CompanyRegistrationNumber == companyRegistrationNumber);

    //todo: teste this in the insomnia
    private async Task<bool> ExistsDriverLicense(string? driverLicenseNumber) =>
        !await _deliveryManRepository.Exists(x => x.DriverLicense.Number == driverLicenseNumber);

    private bool IsDigitsOnly(string? str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return false;
        }

        foreach (char c in str)
        {
            if (c < '0' || c > '9')
                return false;
        }

        return true;
    }
}