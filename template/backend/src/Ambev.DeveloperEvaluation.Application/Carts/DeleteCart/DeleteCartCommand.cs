using Ambev.DeveloperEvaluation.Common.Validation;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

/// <summary>
/// Command for deleting a cart.
/// </summary>
public class DeleteCartCommand : IRequest<DeleteCartResult>
{
    public Guid Id { get; set; }

    public DeleteCartCommand()
    {
    }

    public DeleteCartCommand(Guid id)
    {
        Id = id;
    }

    public ValidationResultDetail Validate()
    {
        var validator = new DeleteCartCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}