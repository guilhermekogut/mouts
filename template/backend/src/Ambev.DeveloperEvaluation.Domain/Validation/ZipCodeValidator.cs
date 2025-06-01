using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class ZipCodeValidator : AbstractValidator<string>
{
    public ZipCodeValidator()
    {
        RuleFor(zipCode => zipCode)
            .NotEmpty().WithMessage("The zipcode cannot be empty.")
            .Matches(@"^\d{5}-\d{3}$").WithMessage("The zip code format is not valid.");
    }
}
