using Ambev.DeveloperEvaluation.Common.Validation;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Command for deleting a product.
/// </summary>
public class DeleteProductCommand : IRequest<DeleteProductResult>
{
    public Guid Id { get; set; }

    public DeleteProductCommand()
    {

    }

    public DeleteProductCommand(Guid id)
    {
        Id = id;
    }
    public ValidationResultDetail Validate()
    {
        var validator = new DeleteProductCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}