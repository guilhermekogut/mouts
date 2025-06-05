using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.ListSales
{
    public class ListSalesRequestValidator : AbstractValidator<ListSalesRequest>
    {
        public ListSalesRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Size must be greater than 0.");
        }
    }
}