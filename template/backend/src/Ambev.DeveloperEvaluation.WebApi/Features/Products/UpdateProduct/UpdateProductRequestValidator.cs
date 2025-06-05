using Ambev.DeveloperEvaluation.Domain.Validation;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Validator for UpdateProductRequest that defines validation rules for product update.
/// </summary>
public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or positive.");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000);

        RuleFor(p => p.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(100);

        RuleFor(p => p.Image)
            .SetValidator(new UrlValidator());
    }
}