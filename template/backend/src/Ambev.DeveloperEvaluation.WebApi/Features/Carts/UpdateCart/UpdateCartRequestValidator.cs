using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    /// <summary>
    /// Validator for UpdateCartRequest.
    /// </summary>
    public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
    {
        public UpdateCartRequestValidator()
        {

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Products)
                .NotNull().WithMessage("Products list is required.")
                .Must(p => p.Count > 0).WithMessage("At least one product must be in the cart.")
                .Must(products => products.Select(prod => prod.ProductId).Distinct().Count() == products.Count)
                .WithMessage("Duplicate products are not allowed in the cart.");

            RuleForEach(x => x.Products)
                .SetValidator(new CartProductItemRequestValidator());
        }
    }
}