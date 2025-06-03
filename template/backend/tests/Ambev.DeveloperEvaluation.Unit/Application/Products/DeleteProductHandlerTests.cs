using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

using FluentAssertions;

using FluentValidation;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the DeleteProductHandler class.
/// </summary>
public class DeleteProductHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly DeleteProductHandler _handler;

    public DeleteProductHandlerTests()
    {
        _handler = new DeleteProductHandler(_productRepository);
    }

    [Fact(DisplayName = "Given valid product id When deleting product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = DeleteProductHandlerTestData.GenerateValidCommand();
        _productRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Id.Should().Be(command.Id);
    }

    [Fact(DisplayName = "Given invalid product id When deleting product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = DeleteProductHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}