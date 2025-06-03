using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListCategories
{
    public class ListCategoriesRequestValidator : AbstractValidator<ListCategoriesRequest>
    {
        public ListCategoriesRequestValidator()
        {
            // Name is optional, but if provided, must not be empty or whitespace
            When(x => x.Name != null, () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Category name filter cannot be empty.");
            });

            // Order is optional, but if provided, must be "asc" or "desc"
            When(x => x.Order != null, () =>
            {
                RuleFor(x => x.Order)
                    .Must(o => o == "asc" || o == "desc")
                    .WithMessage("Order must be 'asc' or 'desc'.");
            });
        }
    }
}