using FluentValidation;

using Mottu.Api.Application.Models;

namespace Mottu.Api.Application.Validators;

public class PatchRentRequestValidator : AbstractValidator<PatchRentRequest>
{
    public PatchRentRequestValidator()
    {
        RuleFor(x => x.ReturnDate)
            .Must(BeAValidDate).WithMessage("Data de devolução inválida, a data não pode ser nula e deve ser no futuro");
    }

    private bool BeAValidDate(DateTime? date) =>
        date != null && date > DateTime.Today;
}