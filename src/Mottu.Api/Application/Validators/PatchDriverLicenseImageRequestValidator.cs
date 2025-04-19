using FluentValidation;

using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.Validators;

public class PatchDriverLicenseImageRequestValidator : AbstractValidator<PatchDriverLicenseImageRequest>
{
    public PatchDriverLicenseImageRequestValidator()
    {
        RuleFor(x => x.DriverLicenseImage)
            .NotNull().NotEmpty().WithMessage("Imagem não pode ser nula ou vazia");
    }
}