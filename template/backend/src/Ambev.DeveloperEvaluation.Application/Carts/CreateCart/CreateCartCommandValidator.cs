using Ambev.DeveloperEvaluation.Application.Carts.Common;

using FluentValidation;
namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    /// <summary>
    /// Validator for CreateCartCommand.
    /// </summary>
    public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
    {
        public CreateCartCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Products)
                .NotNull().WithMessage("Products list is required.")
                .Must(p => p.Count > 0).WithMessage("At least one product must be added to the cart.");

            RuleForEach(x => x.Products).SetValidator(new CartProductItemRequestValidator());
        }
    }
}
