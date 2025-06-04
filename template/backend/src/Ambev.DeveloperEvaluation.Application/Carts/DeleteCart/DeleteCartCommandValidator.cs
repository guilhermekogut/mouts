using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

/// <summary>
/// Validator for DeleteCartCommand.
/// </summary>
public class DeleteCartCommandValidator : AbstractValidator<DeleteCartCommand>
{
    public DeleteCartCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Cart Id is required.");
    }
}