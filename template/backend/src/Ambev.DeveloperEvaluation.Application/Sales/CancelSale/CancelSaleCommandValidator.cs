using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Validator for CancelSaleCommand.
    /// </summary>
    public class CancelSaleCommandValidator : AbstractValidator<CancelSaleCommand>
    {
        public CancelSaleCommandValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty().WithMessage("SaleId is required.");

            RuleFor(x => x.AuthenticatedUserId)
                .NotEmpty().WithMessage("AuthenticatedUserId is required.");
        }
    }
}