using Ambev.DeveloperEvaluation.Application.Carts.Common;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    /// <summary>
    /// Validator for UpdateCartCommand.
    /// </summary>
    public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
    {
        public UpdateCartCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Cart Id is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Products)
                .NotNull().WithMessage("Products list is required.")
                .Must(p => p.Count > 0).WithMessage("At least one product must be in the cart.");

            RuleForEach(x => x.Products).SetValidator(new CartProductItemRequestValidator());
        }
    }
}