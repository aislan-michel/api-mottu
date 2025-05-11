using FluentValidation;

using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Entities;
using Mottu.Api.Domain.Interfaces;

namespace Mottu.Api.Application.Validators;

public class PostDeliveryManRequestValidator : AbstractValidator<PostDeliveryManRequest>
{
    private readonly IRepository<DeliveryMan> _deliveryManRepository;
    private readonly string[] _validDriverLicenseTypes = [ "A", "B", "A+B" ];

    public PostDeliveryManRequestValidator(
        IRepository<DeliveryMan> deliveryManRepository)
    {
        _deliveryManRepository = deliveryManRepository;

        RuleFor(x => x.DriverLicenseType)
            .Must(BeAValidDriverLicenseType).WithMessage($"Tipo da CNH inválida, tipos válidos são: {string.Join(", ", _validDriverLicenseTypes)}");

        RuleFor(x => x.CompanyRegistrationNumber)
            .Must(ExistsCompanyRegistrationNumber).WithMessage(x => $"Entregador com a CNPJ {x.CompanyRegistrationNumber} já cadastrado");

        RuleFor(x => x.DriverLicense)
            .Must(ExistsDriverLicense).WithMessage(x => $"Entregador com a CNH {x.DriverLicense} já cadastrado");
    }

    private bool BeAValidDriverLicenseType(string? type) => 
        _validDriverLicenseTypes.Contains(type);

    private bool ExistsCompanyRegistrationNumber(string? companyRegistrationNumber) =>
        !_deliveryManRepository.Exists(x => x.CompanyRegistrationNumber == companyRegistrationNumber);
    
    //todo: teste this in the insomnia
    private bool ExistsDriverLicense(string? driverLicenseNumber) =>
        !_deliveryManRepository.Exists(x => x.DriverLicense.Number == driverLicenseNumber);
}