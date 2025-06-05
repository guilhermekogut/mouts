using Ambev.DeveloperEvaluation.Domain.Repositories;

using FluentValidation;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Handler for processing DeleteProductCommand requests.
/// </summary>
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new DeleteProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var deleted = await _productRepository.DeleteAsync(command.Id, cancellationToken);

        return new DeleteProductResult
        {
            Id = command.Id,
            Success = deleted,
            Message = deleted ? "Product deleted successfully." : "Product not found."
        };
    }
}