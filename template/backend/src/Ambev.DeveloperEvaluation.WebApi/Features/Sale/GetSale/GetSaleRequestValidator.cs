using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale
{
    public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
    {
        public GetSaleRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Sale Id is required.");
        }
    }
}