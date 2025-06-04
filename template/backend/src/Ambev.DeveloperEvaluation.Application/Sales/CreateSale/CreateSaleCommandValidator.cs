using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Validator for CreateSaleCommand.
    /// </summary>
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(x => x.CartId)
                .NotEmpty().WithMessage("CartId is required.");
            RuleFor(x => x.AuthenticatedUserId)
                .NotEmpty().WithMessage("AuthenticatedUserId is required.");
        }
    }
}