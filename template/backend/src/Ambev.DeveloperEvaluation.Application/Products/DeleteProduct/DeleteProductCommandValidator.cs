﻿using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Validator for DeleteProductCommand.
/// </summary>
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product Id is required.");
    }
}