using Ambev.DeveloperEvaluation.Domain.Validation;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand that defines validation rules for product creation.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
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

        RuleFor(p => p.Image).SetValidator(new UrlValidator());
    }
}