using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale
{
    /// <summary>
    /// Validator for UpdateSaleRequest.
    /// </summary>
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty().WithMessage("SaleId is required.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");
        }
    }
}