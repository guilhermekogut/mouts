using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProductsByCategory
{
    public class ListProductsByCategoryRequestValidator : AbstractValidator<ListProductsByCategoryRequest>
    {
        public ListProductsByCategoryRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Size must be greater than 0.");

            RuleFor(x => x.Title)
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.Image)
                .MaximumLength(300);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0).When(x => x.MaxPrice.HasValue);

            RuleFor(x => x.MinPrice)
                .LessThanOrEqualTo(x => x.MaxPrice)
                .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);

            RuleFor(x => x.RatingRate)
                .InclusiveBetween(0, 5).When(x => x.RatingRate.HasValue);

            RuleFor(x => x.MinRatingRate)
                .InclusiveBetween(0, 5).When(x => x.MinRatingRate.HasValue);

            RuleFor(x => x.MaxRatingRate)
                .InclusiveBetween(0, 5).When(x => x.MaxRatingRate.HasValue);

            RuleFor(x => x.MinRatingRate)
                .LessThanOrEqualTo(x => x.MaxRatingRate)
                .When(x => x.MinRatingRate.HasValue && x.MaxRatingRate.HasValue);

            RuleFor(x => x.RatingCount)
                .GreaterThanOrEqualTo(0).When(x => x.RatingCount.HasValue);

            RuleFor(x => x.MinRatingCount)
                .GreaterThanOrEqualTo(0).When(x => x.MinRatingCount.HasValue);

            RuleFor(x => x.MaxRatingCount)
                .GreaterThanOrEqualTo(0).When(x => x.MaxRatingCount.HasValue);

            RuleFor(x => x.MinRatingCount)
                .LessThanOrEqualTo(x => x.MaxRatingCount)
                .When(x => x.MinRatingCount.HasValue && x.MaxRatingCount.HasValue);
        }
    }
}