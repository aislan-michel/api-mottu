using FluentValidation.Results;

namespace Mottu.Api.Extensions;

public static class FluentValidationExtensions
{
    public static IEnumerable<string> GetErrorMessages(this ValidationResult validationResult)
    {
        return validationResult.Errors.Select(x => x.ErrorMessage);
    }
}