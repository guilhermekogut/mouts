using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
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
/// Contains unit tests for the CreateProductHandler class.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _handler = new CreateProductHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid product data When creating product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        var product = new Product { Id = Guid.NewGuid(), Title = command.Title, Price = command.Price, Description = command.Description, Category = command.Category, Image = command.Image };
        var result = new CreateProductResult { Id = product.Id, Title = product.Title, Price = product.Price, Description = product.Description, Category = product.Category, Image = product.Image };

        _mapper.Map<Product>(command).Returns(product);
        _mapper.Map<CreateProductResult>(product).Returns(result);
        _productRepository.CreateAsync(product, Arg.Any<CancellationToken>()).Returns(product);

        // When
        var createProductResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createProductResult.Should().NotBeNull();
        createProductResult.Id.Should().Be(product.Id);
        await _productRepository.Received(1).CreateAsync(product, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid product data When creating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}