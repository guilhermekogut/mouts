using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    /// <summary>
    /// Validator for CreateCartRequest.
    /// </summary>
    public class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
    {
        public CreateCartRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Products)
                .NotNull().WithMessage("Products list is required.")
                .Must(p => p.Count > 0).WithMessage("At least one product must be added to the cart.");

            RuleForEach(x => x.Products)
                .SetValidator(new CartProductItemRequestValidator());
        }
    }
}