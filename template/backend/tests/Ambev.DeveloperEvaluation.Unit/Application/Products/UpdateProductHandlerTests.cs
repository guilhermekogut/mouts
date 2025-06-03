using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

using AutoMapper;

using FluentAssertions;

using FluentValidation;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the UpdateProductHandler class.
/// </summary>
public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _handler = new UpdateProductHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid product data When updating product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = new Product { Id = command.Id, Title = command.Title, Price = command.Price, Description = command.Description, Category = command.Category, Image = command.Image };
        var result = new UpdateProductResult { Id = product.Id, Title = product.Title, Price = product.Price, Description = product.Description, Category = product.Category, Image = product.Image };

        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map(command, product).Returns(product);
        _productRepository.UpdateAsync(product, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<UpdateProductResult>(product).Returns(result);

        // When
        var updateProductResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateProductResult.Should().NotBeNull();
        updateProductResult.Id.Should().Be(product.Id);
        await _productRepository.Received(1).UpdateAsync(product, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid product data When updating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = UpdateProductHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}