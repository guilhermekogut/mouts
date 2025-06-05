using Ambev.DeveloperEvaluation.Domain.Repositories;

using FluentValidation;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

/// <summary>
/// Handler for processing DeleteCartCommand requests.
/// </summary>
public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResult>
{
    private readonly ICartRepository _cartRepository;

    public DeleteCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<DeleteCartResult> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
    {
        var validator = new DeleteCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = await _cartRepository.GetByIdAsync(command.Id, cancellationToken);
        if (cart == null)
        {
            return new DeleteCartResult
            {
                Id = command.Id,
                Success = false,
                Message = "Cart not found."
            };
        }

        await _cartRepository.DeleteAsync(command.Id, cancellationToken);

        return new DeleteCartResult
        {
            Id = command.Id,
            Success = true,
            Message = "Cart deleted successfully."
        };
    }
}