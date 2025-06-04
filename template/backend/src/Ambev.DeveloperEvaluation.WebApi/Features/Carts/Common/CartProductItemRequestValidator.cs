using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

/// <summary>
/// Validator for CartProductItemRequest.
/// </summary>
public class CartProductItemRequestValidator : AbstractValidator<CartProductItemRequest>
{
    public CartProductItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(20).WithMessage("Quantity must not exceed 20 for a single product.");
    }
}
